using System.Collections.Generic;

namespace DataSync.Core
{
    /// <summary>
    /// 表
    /// </summary>
    public class Table
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public List<Column> Columns { get; set; } = new List<Column>();

        /// <summary>
        /// 需要导入的数据列
        /// </summary>
        public List<Column> SyncColumn { get; set; } = new List<Column>();

        /// <summary>
        /// 索引
        /// </summary>
        public Dictionary<string, Index> Indexes { get; set; } = new Dictionary<string, Index>();
    }
}