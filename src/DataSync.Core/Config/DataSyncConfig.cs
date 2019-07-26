using System.Collections.Generic;

namespace DataSync.Core.Config
{
    public class DataSyncConfig
    {
        /// <summary>
        /// 配置版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 并行任务数
        /// </summary>
        public int Concurrent { get; set; }

        /// <summary>
        /// 配置任务
        /// </summary>
        public List<SyncConfig> Sync { get; set; }
    }
}