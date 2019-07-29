using System.Collections.Generic;

namespace DataSync.Core
{
    public interface IDataSyncReader
        // : IEnumerable<Dictionary<string, object>>
    {
        string Name { get; }

        List<Dictionary<string, object>> GetData();
    }
}