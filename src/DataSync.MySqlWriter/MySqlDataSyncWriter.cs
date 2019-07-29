using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DataSync.Core;
using DataSync.MySqlWriter.Config;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace DataSync.MySqlWriter
{
    public class MySqlDataSyncWriter : IDataSyncWriter
    {
        private readonly MySqlWriterConfig _config;

        public string Name => "mysql";

        public MySqlDataSyncWriter()
        {
        }

        public MySqlDataSyncWriter(string config)
        {
            _config = JsonConvert.DeserializeObject<MySqlWriterConfig>(config);
        }

        public async Task EnsureDatabaseAndTableCreated(List<Table> tables)
        {
            await using var conn = new MySqlConnection(
                $"Database='mysql';Data Source={_config.Host};password={_config.Password};User ID={_config.User};Port={_config.Port};");
            await conn.ExecuteAsync(
                $"CREATE SCHEMA IF NOT EXISTS {_config.Database} DEFAULT CHARACTER SET utf8mb4;");

            foreach (var table in tables)
            {
                try
                {
                    var builder =
                        new StringBuilder(
                            $"CREATE TABLE IF NOT EXISTS `{_config.Database}`.`{table.Name}` ({Environment.NewLine}");

                    foreach (var column in table.Columns)
                    {
                        var DEFAULT =
                            $"{(string.IsNullOrWhiteSpace(column.Default) ? "" : (" DEFAULT " + column.Default))}";
                        builder.Append(
                            $"\t`{column.Name}` {column.Type} {(column.Nullable ? "NULL" : "NOT NULL")}{DEFAULT}{(column.AutoIncrement ? " AUTO_INCREMENT" : "")},{Environment.NewLine}");
                    }

                    foreach (var index in table.Indexes)
                    {
                        var columnNames = string.Join(", ", index.Value.Columns.Select(c => $"`{c.Name}`"));

                        builder.Append(index.Key == "PRIMARY"
                            ? $"\tPRIMARY KEY ({columnNames}),{Environment.NewLine}"
                            : $"\t{(index.Value.Unique ? "UNIQUE " : "")}KEY `{index.Key}` ({columnNames}),{Environment.NewLine}");
                    }

                    builder.Remove(builder.Length - 2, 2);
                    builder.Append($"{Environment.NewLine}) ENGINE = InnoDB DEFAULT CHARSET = utf8mb4;");
                    var sql = builder.ToString();
                    await conn.ExecuteAsync(sql);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR: {e}");
                }
            }
        }

        public async Task WriterAsync(string table, List<string> columns, List<IDictionary<string, object>> data)
        {
            await using var conn = new MySqlConnection(
                $"Database='mysql';Data Source={_config.Host};password={_config.Password};User ID={_config.User};Port={_config.Port};");
            var columnSql = string.Join(", ", columns.Select(x => $"`{x}`"));
            var values = string.Join(", ", columns.Select(x => $"@{x}".Replace(" ", "_")));
            foreach (var entry in data)
            {
                var containsSpaceValues = new List<KeyValuePair<string, object>>();
                foreach (var kv in entry)
                {
                    if (kv.Key.Contains(" "))
                    {
                        containsSpaceValues.Add(new KeyValuePair<string, object>(kv.Key.Replace(" ", "_"), kv.Value));
                    }
                }

                foreach (var kv in containsSpaceValues)
                {
                    entry.Add(kv.Key, kv.Value);
                }
            }

            var builder =
                new StringBuilder(
                    $"INSERT IGNORE INTO `{_config.Database}`.`{table}` ({columnSql}) VALUES ({values});");
            var insertSql = builder.ToString();
            await conn.ExecuteAsync(insertSql, data);
        }
    }
}