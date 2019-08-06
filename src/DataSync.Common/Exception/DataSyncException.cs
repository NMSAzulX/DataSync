namespace DataSync.Common.Exception
{
    public class DataSyncException : System.Exception
    {
        public IErrorCode ErrorCode { get; }

        public DataSyncException(string msg) : base(msg)
        {
        }

        public DataSyncException(IErrorCode errorCode, string msg) : base(msg)
        {
            ErrorCode = errorCode;
        }
    }
}