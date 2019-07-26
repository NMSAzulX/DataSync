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
        public List<Column> Columns { get; set; }
        
        /// <summary>
        /// 索引
        /// </summary>
        public List<Index> Indexes { get; set; }
    }
}