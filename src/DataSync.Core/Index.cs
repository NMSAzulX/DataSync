using System.Collections.Generic;

namespace DataSync.Core
{
    /// <summary>
    /// 索引
    /// </summary>
    public class Index
    {
        /// <summary>
        /// 索引名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 是否唯一索引
        /// </summary>
        public bool Unique { get; set; }
        
        /// <summary>
        /// 索引包含的列
        /// </summary>
        public List<Column> Columns { get; set; }
    }
}