using System;
using System.Collections.Generic;
using System.Data;
using DataSync.Common;
using DataSync.Common.Exception;
using DataSync.Common.Plugin;
using DataSync.RDBMS.Util.Reader.Util;
using DataSync.RDBMS.Util.Writer.Util;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;

namespace DataSync.RDBMS.Util.Reader
{
    public class RdbmsReader
    {
        public class Job
        {
            private readonly ILogger _logger = Log.ForContext<Job>();

            public Job(DataBaseType dataBaseType)
            {
                OriginalConfPretreatmentUtil.DatabaseType = dataBaseType;
                SingleTableSplitUtil.DatabaseType = dataBaseType;
            }

            public void Init(IConfiguration configuration)
            {
                OriginalConfPretreatmentUtil.Pretreat(configuration);
                _logger.Debug(
                    $"After job init(), job config now is:[\n{JsonConvert.SerializeObject(configuration)}\n]");
            }

            public void PreCheck(IConfiguration originalConfig, DataBaseType dataBaseType)
            {
            }

            public List<IConfiguration> Split(IConfiguration originalConfig,
                int adviceNumber)
            {
                return ReaderSplitUtil.Split(originalConfig, adviceNumber);
            }

            public void Post(IConfiguration configuration)
            {
                // do nothing
            }

            public void Destroy(IConfiguration configuration)
            {
                // do nothing
            }
        }

        public class Task
        {
            private readonly ILogger _logger = Log.ForContext<Task>();

            private int _taskGroupId = -1;
            private int _taskId = -1;
            private DataBaseType _dataBaseType;
            private string _connectionString;

            public Task(DataBaseType dataBaseType, int taskGroupId = -1, int taskId = -1)
            {
                _dataBaseType = dataBaseType;
                _taskGroupId = taskGroupId;
                _taskId = taskId;
            }

            public void Init(IConfiguration configuration)
            {
                /* for database connection */

                _connectionString = configuration[Key.ConnectionString];
            }

            public void StartRead(IConfiguration configuration, IRecordSender recordSender, int fetchSize)
            {
                string querySql = configuration[Key.QuerySql];
                string table = configuration[Key.Table];
                var type = Type.GetType(_dataBaseType.DriveClassTypeName);
                if (type == null)
                {
                    throw new DataSyncException("数据驱动类型不正确" + _dataBaseType.DriveClassTypeName);
                }

                var connection = (IDbConnection) Activator.CreateInstance(type);
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                DBUtil.ConfigureSession(connection, configuration, _dataBaseType);
            }

            public void Post(IConfiguration configuration)
            {
                // do nothing
            }

            public void Destroy(IConfiguration configuration)
            {
                // do nothing
            }
        }
    }
}