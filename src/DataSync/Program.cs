using System;
using System.IO;
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


            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" && args.Length == 0)
            {
                args = new[] {"sample.json"};
            }
            else
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Use datasync [x.json]");
                    return;
                }

                if (!File.Exists(args[0]))
                {
                    Console.WriteLine($"File {args[0]} not found");
                    return;
                }
            }

            SyncFlow flow = new SyncFlow();
            await flow.RunAsync(args[0]);
        }
    }
}