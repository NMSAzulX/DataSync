using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataSync.Core.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataSync.Core
{
    public class SyncFlow
    {
        private readonly Dictionary<string, Type> _readerDict = new Dictionary<string, Type>();
        private readonly Dictionary<string, Type> _writerDict = new Dictionary<string, Type>();

        public Task RunAsync(string file)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetTypes());
            }

            var readerBaseType = typeof(IDataSyncReader);

            foreach (var type in types)
            {
                if (readerBaseType.IsAssignableFrom(type))
                {
                    var reader = (IDataSyncReader) Activator.CreateInstance(type);
                    _readerDict.Add(reader.Name, type);
                }
            }

            var json = File.ReadAllText(file);
            var dataSync = JsonConvert.DeserializeObject<DataSyncConfig>(json);

            
            // TODO: check all reader, writer exits
            
            if (dataSync.Sync.Count > 0)
            {
                var obj = JsonConvert.DeserializeObject<JObject>(json);
                foreach (var config in dataSync.Sync)
                {
                    var readerType = config.Reader.Type;
                    var writerType = config.Writer.Type;
                    var reader = _readerDict[readerType];
                    var writer = _writerDict[writerType];

                }
                Parallel.ForEach(dataSync.Sync, new ParallelOptions
                {
                    MaxDegreeOfParallelism = dataSync.Concurrent
                }, sync =>
                {
                    
                });
            }

            return Task.CompletedTask;
        }
    }
}