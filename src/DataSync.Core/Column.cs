using System.Collections.Generic;

namespace DataSync.Core
{
    public class Column
    {
        private readonly IDictionary<string, object> _dict;

        public Column(IDictionary<string, object> dict)
        {
            _dict = dict;
        }

        public bool AutoIncrement => _dict["extra"] != null && _dict["extra"].ToString().Contains("auto_increment");

        /// <summary>
        /// 列名
        /// </summary>
        public string Name => _dict["name"]?.ToString();

        /// <summary>
        /// 数据类型
        /// </summary>
        public string Type => _dict["type"]?.ToString();

        /// <summary>
        /// 默认值
        /// </summary>
        public string Default => _dict.ContainsKey("default") ? _dict["default"]?.ToString() : null;

        /// <summary>
        /// 是否可以为空
        /// </summary>
        public bool Nullable
        {
            get
            {
                var nullable = _dict["is_nullable"]?.ToString()?.ToLower();
                if (string.IsNullOrWhiteSpace(nullable))
                {
                    return true;
                }
                else
                {
                    return nullable == "1" ||
                           nullable == "yes" ||
                           nullable == "y";
                }
            }
        }
    }
}