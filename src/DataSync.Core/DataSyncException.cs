using System;

namespace DataSync.Core
{
    public class DataSyncException : Exception
    {
        public DataSyncException(string msg) : base(msg)
        {
        }
    }
}