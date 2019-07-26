using System.Collections.Generic;

namespace DataSync.Core
{
    public class Column : Dictionary<string, string>
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name => ContainsKey("name") ? this["name"] : null;

        /// <summary>
        /// 数据类型
        /// </summary>
        public string Type => ContainsKey("type") ? this["type"] : null;

        /// <summary>
        /// 默认值
        /// </summary>
        public string Default => ContainsKey("type") ? this["type"] : null;

        /// <summary>
        /// 是否可以为空
        /// </summary>
        public bool Nullable
        {
            get
            {
                if (!ContainsKey("is_nullable") || string.IsNullOrWhiteSpace(this["is_nullable"]))
                {
                    return true;
                }
                else
                {
                    return this["is_nullable"] == "1" || this["is_nullable"].ToLower() == "yes" ||
                           this["is_nullable"].ToLower() == "y";
                }
            }
        }
    }
}