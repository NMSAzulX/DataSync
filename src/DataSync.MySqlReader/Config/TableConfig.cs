using System.Collections.Generic;

namespace DataSync.MySqlReader.Config
{
    public class TableConfig
    {
        public string Name { get; set; }

        public string SplitPk { get; set; }

        public string Where { get; set; }

        public List<string> Column { get; set; } = new List<string>();
    }
}