namespace DataSync.Common.Exception
{
    public interface IErrorCode
    {
        string Code { get; }
        string Description { get; }
    }
}