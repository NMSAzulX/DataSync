using System;
using DataSync.Common;
using DataSync.Common.Exception;
using DataSync.Core.Exception;
using Microsoft.Extensions.Configuration;

namespace DataSync.Core
{
    public class Engine
    {
        public static void Entry(params string[] args)
        {
            var configuration = GetConfiguration(args);
            var jobIdString = configuration["jobid"];
            var runtimeMode = configuration["mode"];
            long jobId = -1;
            if (!"-1".Equals(jobIdString, StringComparison.OrdinalIgnoreCase))
            {
                jobId = long.Parse(jobIdString);
            }

            var isStandAloneMode = "standalone".Equals(runtimeMode, StringComparison.OrdinalIgnoreCase);
            if (!isStandAloneMode && jobId == -1)
            {
                // 如果不是 standalone 模式，那么 jobId 一定不能为-1
                throw new DataSyncException(FrameworkErrorCodes.ConfigError, "非 standalone 模式必须在 URL 中提供有效的 jobId.");
            }

            ValidateConfiguration(configuration);
            Engine engine = new Engine();
            engine.Start(configuration);
        }

        private void Start(IConfiguration configuration)
        {
        }

        private static IConfiguration GetConfiguration(params string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddCommandLine(args);
            configurationBuilder.AddEnvironmentVariables();
            var configuration = configurationBuilder.Build();
            var jobPath = configuration["job"];
            configurationBuilder.AddJsonFile(jobPath);
            configuration = configurationBuilder.Build();
            return configuration;
        }

        public static void ValidateConfiguration(IConfiguration configuration)
        {
            Validate.IsTrue(configuration != null, "");

            CoreValidate(configuration);

            PluginValidate(configuration);

            JobValidate(configuration);
        }

        private static void CoreValidate(IConfiguration configuration)
        {
           // todo:
        }
        
        private static void PluginValidate(IConfiguration configuration)
        {
            // todo:
        }
        
        private static void JobValidate(IConfiguration configuration)
        {
            // todo:
        }
    }
}