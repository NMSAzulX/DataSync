//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using DataSync.Core;
//
//namespace DataSync.CsvWriter
//{
//    public class CsvDataSyncWriter : IDataSyncWriter
//    {
//        public string Name => "csv";
//        
//        public Task EnsureDatabaseAndTableCreated(List<Table> tables)
//        {
//            throw new NotImplementedException();
//        }
//
//        public Task WriterAsync(string table, List<string> columns, List<IDictionary<string, object>> data)
//        {
//            throw new NotImplementedException();
//        }
//
//
//        public CsvDataSyncWriter()
//        {
//        }
//
//        public CsvDataSyncWriter(string config)
//        {
//        }
//    }
//}