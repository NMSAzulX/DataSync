using System.Collections.Generic;
using DataSync.Core.Config;

namespace DataSync.MySqlReader.Config
{
    public class MyReaderConfig : ReaderConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public List<string> Sql { get; set; }
        public string Database { get; set; }
        public List<TableConfig> Table { get; set; }
    }
}