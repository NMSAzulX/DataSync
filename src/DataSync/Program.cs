using System;
using System.Threading.Tasks;
using DataSync.Core;
using Serilog;
using Serilog.Events;

namespace DataSync
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configure = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Verbose()
#else
				.MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console().WriteTo
                .RollingFile("datasync.log");
            Log.Logger = configure.CreateLogger();

            try
            {
                args = new[] {"sample.json"};
                await Engine.Entry(args);
            }
            catch (Exception e)
            {
                // todo: 打印更智能的信息
                Log.Logger.Error(e.ToString());
            }
        }
    }
}