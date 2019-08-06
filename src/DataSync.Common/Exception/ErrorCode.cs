namespace DataSync.Common.Exception
{
    public class ErrorCode : IErrorCode
    {
        public string Code { get; }

        public string Description { get; }

        public ErrorCode(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}