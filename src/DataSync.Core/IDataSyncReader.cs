using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSync.Core
{
    public interface IDataSyncReader
    {
        string Name { get; }

        Task RunAsync();
    }
}