using System.Collections.Generic;
using DataSync.Core;

namespace DataSync.MySqlReader
{
    public class MySqlDataSyncReader: IDataSyncReader
    {
        public string Name => "mysql";
        
        
        
        public List<Dictionary<string, object>> GetData()
        {
            throw new System.NotImplementedException();
        }
    }
}