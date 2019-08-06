using System.Collections.Generic;
using DataSync.Common;
using DataSync.Common.Plugin;
using DataSync.RDBMS.Util;
using DataSync.RDBMS.Util.Reader;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace DataSync.MySqlReader
{
    public class MySqlReader : DataSyncReader
    {
        private static readonly DataBaseType DatabaseType = DataBaseType.MySql;

        public class Job : DataSyncReader.Job
        {
            private readonly ILogger _logger = Log.ForContext<Job>();
            private RdbmsReader.Job _rdbmsReaderJob;

            public override void Init()
            {
                _rdbmsReaderJob = new RdbmsReader.Job(DatabaseType);
                _rdbmsReaderJob.Init(JobConfiguration);

                var userConfiguredFetchSize = JobConfiguration.GetValue<int?>(Constant.FETCH_SIZE);
                if (userConfiguredFetchSize != null)
                {
                    _logger.Warning(
                        "对 mysqlreader 不需要配置 fetchSize, mysqlreader 将会忽略这项配置. 如果您不想再看到此警告,请去除fetchSize 配置.");
                }
            }

            public override void Destroy()
            {
                _rdbmsReaderJob.Destroy(JobConfiguration);
            }

            public override void PreCheck()
            {
                Init();
                _rdbmsReaderJob.PreCheck(JobConfiguration, DatabaseType);
            }

            public override List<IConfiguration> Split(int adviceNumber)
            {
                return _rdbmsReaderJob.Split(JobConfiguration, adviceNumber);
            }
        }

        public class Task : DataSyncReader.Task
        {
            private RdbmsReader.Task _rdbmsReaderTask;


            public override void Init()
            {
                _rdbmsReaderTask =
                    new RdbmsReader.Task(DatabaseType, Group, Id);
                _rdbmsReaderTask.Init(JobConfiguration);
            }


            public override void StartRead(IRecordSender recordSender)
            {
                var fetchSize = JobConfiguration.GetValue<int>(Constant.FETCH_SIZE);

                _rdbmsReaderTask.StartRead(JobConfiguration, recordSender, fetchSize);
            }


            public override void Post()
            {
                _rdbmsReaderTask.Post(JobConfiguration);
            }


            public override void Destroy()
            {
                _rdbmsReaderTask.Destroy(JobConfiguration);
            }
        }
    }
}