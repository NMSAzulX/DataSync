
using DataSync.Common.Exception;

namespace DataSync.Core.Exception
{
    public static class FrameworkErrorCodes
    {
        public static readonly IErrorCode ConfigError = new ErrorCode("Framework-03", "DataSync 引擎配置错误，该问题通常是由于 DataSync 安装错误引起，请联系您的运维解决。");
    }
}