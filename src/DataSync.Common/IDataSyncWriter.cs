//using System.Collections.Generic;
//using System.Threading.Tasks;
//
//namespace DataSync.Core
//{
//    public interface IDataSyncWriter
//    {
//        string Name { get; }
//
//        Task EnsureDatabaseAndTableCreated(List<Table> tables);
//
//        Task WriterAsync(string table, List<string> columns, List<IDictionary<string, object>> data);
//    }
//}