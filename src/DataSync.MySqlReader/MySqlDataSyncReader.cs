using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DataSync.Core;
using DataSync.MySqlReader.Config;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace DataSync.MySqlReader
{
    public class MySqlDataSyncReader : IDataSyncReader
    {
        private readonly MySqlReaderConfig _config;
        private List<Table> _tables;
        private readonly IDataSyncWriter _writer;

        public string Name => "mysql";

        public async Task RunAsync()
        {
            var tables = await GetTablesAsync();
            await _writer.EnsureDatabaseAndTableCreated(tables);

            await using var conn = new MySqlConnection(
                $"Database='{_config.Database}';Data Source={_config.Host};password={_config.Password};User ID={_config.User};Port={_config.Port};");

            foreach (var table in _tables)
            {
                var tableConfig = _config.Table.FirstOrDefault(x => x.Name == table.Name);
                List<string> columns;
                if (tableConfig?.Column == null || tableConfig.Column.Count == 0)
                {
                    columns = table.Columns.Select(x => x.Name).ToList();
                }
                else
                {
                    columns = tableConfig.Column;
                }

                var data = new List<IDictionary<string, object>>();
                var columnSql = string.Join(", ", columns.Select(x => $"`{x}`"));

                // 先取 table 中定义的 where
                var whereSql = tableConfig == null || string.IsNullOrWhiteSpace(tableConfig.Where)
                    ? ""
                    : $"WHERE {tableConfig.Where}";
                // 如果 table 中未定义 where 再取全局设定
                if (string.IsNullOrWhiteSpace(whereSql))
                {
                    whereSql = _config.Where;
                }

                var selectSql = $"SELECT {columnSql} FROM `{_config.Database}`.`{table.Name}` {whereSql}";
                var total = 0;
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                foreach (var entry in await conn.QueryAsync(selectSql))
                {
                    data.Add(entry);

                    if (data.Count == 1000)
                    {
                        await _writer.WriterAsync(table.Name, columns, data);
                        data.Clear();
                        total += 1000;
                    }
                }

                if (data.Count > 0)
                {
                    await _writer.WriterAsync(table.Name, columns, data);
                    total += data.Count;
                    data.Clear();
                }

                stopwatch.Stop();
                Console.WriteLine(
                    $"Database: {_config.Database}, Table: {table.Name}, Synced: {total} row, Cost: {stopwatch.ElapsedMilliseconds / 1000} second");
            }
        }

        public MySqlDataSyncReader()
        {
        }

        public MySqlDataSyncReader(string config, IDataSyncWriter writer)
        {
            _config = JsonConvert.DeserializeObject<MySqlReaderConfig>(config);
            _writer = writer;
        }

        /// <summary>
        /// 查询所有表的定义用于创建到目标
        /// TODO: 即使配置文件只使用 table 定义了只导入特定的某些列，但表结构还是原样复制，主要考虑到缺失的列如果在索引中如何修正索引的创建？
        /// </summary>
        /// <returns></returns>
        private async Task<List<Table>> GetTablesAsync()
        {
            if (_tables == null)
            {
                _tables = new List<Table>();

                if (_config.Sql != null && _config.Sql.Count > 0)
                {
                    // TODO:
                }
                else
                {
                    List<string> tableNames;
                    await using var conn = new MySqlConnection(
                        $"Database='{_config.Database}';Data Source={_config.Host};password={_config.Password};User ID={_config.User};Port={_config.Port};");
                    // 如果配置中未配置具体的表，则表示需要整库同步
                    if (_config.Table == null || _config.Table.Count == 0)
                    {
                        tableNames =
                            (await conn.QueryAsync<string>($"USE {_config.Database}; SHOW TABLES;")).ToList();
                    }
                    else
                    {
                        tableNames = _config.Table.Select(x => x.Name).ToList();
                    }

                    if (tableNames.Count > 0)
                    {
                        foreach (var tableName in tableNames)
                        {
                            var queryColumnsSql =
                                $"SELECT COLUMN_NAME AS name, COLUMN_TYPE AS type, COLUMN_DEFAULT AS `default`, IS_NULLABLE AS is_nullable, EXTRA AS extra FROM information_schema.COLUMNS WHERE table_name = '{tableName}' AND table_schema='{_config.Database}'";
                            var columns = (await conn.QueryAsync<dynamic>(queryColumnsSql))
                                .Select(x => new Column((IDictionary<string, object>) x)).ToList();
                            var table = new Table
                            {
                                Name = tableName,
                                Columns = columns
                            };
                            var indexes = (await conn.QueryAsync<Index>(
                                    $"SHOW INDEX FROM `{_config.Database}`.`{tableName}`")
                                ).ToList();

                            foreach (var index in indexes)
                            {
                                if (!table.Indexes.ContainsKey(index.Key_name))
                                {
                                    table.Indexes.Add(index.Key_name, new Core.Index());
                                }

                                table.Indexes[index.Key_name].Unique = !index.Non_unique;
                                var column = table.Columns.First(x => x.Name == index.Column_name);
                                table.Indexes[index.Key_name].Columns.Add(column);
                            }

                            if (_config.Table == null || _config.Table.Count == 0)
                            {
                                table.SyncColumn = table.Columns;
                            }
                            else
                            {
                                var tableConfig = _config.Table.First(x => x.Name == tableName);
                                foreach (var column in tableConfig.Column)
                                {
                                    var originalColumn = table.Columns.FirstOrDefault(x => x.Name == column);
                                    if (originalColumn == null)
                                    {
                                        throw new DataSyncException("配置的列不在数据库表中");
                                    }

                                    table.SyncColumn.Add(originalColumn);
                                }
                            }

                            _tables.Add(table);
                        }
                    }
                }
            }

            return _tables;
        }

        class Index
        {
            public string Key_name { get; set; }
            public string Column_name { get; set; }
            public bool Non_unique { get; set; }
        }
    }
}