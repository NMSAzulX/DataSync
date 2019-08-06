using System.Collections.Generic;
using System.Threading.Tasks;
using DataSync.Common.Plugin;
using Microsoft.Extensions.Configuration;

namespace DataSync.Common
{
    public class DataSyncReader
    {
        public abstract class Job : AbstractJobPlugin
        {
            public abstract List<IConfiguration> Split(int adviceNumber);
        }

        public abstract class Task : AbstractTaskPlugin
        {
            public abstract void StartRead(IRecordSender recordSender);
        }
    }
}