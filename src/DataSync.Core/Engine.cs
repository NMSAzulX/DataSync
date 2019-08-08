using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataSync.Common;
using DataSync.Common.Exception;
using DataSync.Core.Exception;
using Microsoft.Extensions.Configuration;
using Natasha.Operator;
using Serilog;

namespace DataSync.Core
{
    public class Engine
    {
        public static async Task Entry(params string[] args)
        {
            var configuration = GetConfiguration(args);
            Engine engine = new Engine();
            await engine.StartAsync(configuration);
        }

        private async Task StartAsync(IConfiguration configuration)
        {
            var a = CloneOperator.Clone((ConfigurationRoot) configuration);
            Log.Logger.Information("DataSync starts job");

            await PreHandlerAsync(a);
        }

        private Task PreHandlerAsync(IConfiguration configuration)
        {
            return Task.CompletedTask;
        }

        private static IConfiguration GetConfiguration(params string[] args)
        {
            var path = args.ElementAtOrDefault(0);
            if (!string.IsNullOrWhiteSpace(path) && File.Exists(path))
            {
                var builder = new ConfigurationBuilder();

                builder.AddEnvironmentVariables();
                builder.AddJsonFile(path);
                var configuration = builder.Build();
                return configuration;
            }
            else
            {
                throw new DataSyncException("任务文件不存在");
            }
        }
    }
}