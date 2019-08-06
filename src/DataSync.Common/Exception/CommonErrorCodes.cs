namespace DataSync.Common.Exception
{
    public static class CommonErrorCodes
    {
        public static readonly IErrorCode ConfigError = new ErrorCode("Common-00", "您提供的配置文件存在错误信息，请检查您的作业配置。");
    }
}