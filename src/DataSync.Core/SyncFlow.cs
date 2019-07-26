using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DataSync.Core.Config;
using Newtonsoft.Json;

namespace DataSync.Core
{
    public class SyncFlow
    {
        private readonly Dictionary<string, IDataSyncReader> _readerDict = new Dictionary<string, IDataSyncReader>();
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
                    _readerDict.Add(reader.Name, reader);
                }
            }

            var json = File.ReadAllText(file);
            var dataSync = JsonConvert.DeserializeObject<DataSyncConfig>(json);

            
            // TODO: check all reader, writer exits
            
            if (dataSync.Sync.Count > 0)
            {
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