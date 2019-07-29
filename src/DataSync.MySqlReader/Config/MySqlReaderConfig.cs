using System.Collections.Generic;
using DataSync.Core.Config;

namespace DataSync.MySqlReader.Config
{
    public class MySqlReaderConfig : ReaderConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        public List<string> Sql { get; set; } = new List<string>();

        public string Database { get; set; }
        
        public string Where { get; set; }

        public List<TableConfig> Table { get; set; } = new List<TableConfig>();
    }
}