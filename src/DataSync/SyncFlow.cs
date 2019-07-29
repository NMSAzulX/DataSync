using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataSync.Core;
using DataSync.Core.Config;
using DataSync.MySqlReader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataSync
{
    public class SyncFlow
    {
        private readonly Dictionary<string, Type> _readerDict = new Dictionary<string, Type>();
        private readonly Dictionary<string, Type> _writerDict = new Dictionary<string, Type>();

        public async Task RunAsync(string file)
        {
            var assemblies = GetGetAssemblies();
            var types = new List<Type>();
            foreach (var assembly in assemblies)
            {
                types.AddRange(assembly.GetTypes());
            }

            var readerBaseType = typeof(IDataSyncReader);
            var writerBaseType = typeof(IDataSyncWriter);
            foreach (var type in types)
            {
                if (readerBaseType.IsAssignableFrom(type))
                {
                    var reader = (IDataSyncReader) Activator.CreateInstance(type);
                    _readerDict.Add(reader.Name, type);
                }

                if (writerBaseType.IsAssignableFrom(type))
                {
                    var writer = (IDataSyncWriter) Activator.CreateInstance(type);
                    _writerDict.Add(writer.Name, type);
                }
            }

            var json = File.ReadAllText(file);
            var dataSync = JsonConvert.DeserializeObject<DataSyncConfig>(json);

            // TODO: check all reader, writer exits

            var syncs = new List<Tuple<IDataSyncReader, IDataSyncWriter>>();
            if (dataSync.Sync.Count > 0)
            {
                var obj = JsonConvert.DeserializeObject<JObject>(json);
                for (int i = 0; i < dataSync.Sync.Count; ++i)
                {
                    var readerType = dataSync.Sync[i].Reader.Type;
                    var writerType = dataSync.Sync[i].Writer.Type;
                    var writerConfig = obj["sync"][i]["writer"].ToString();
                    var writer = (IDataSyncWriter) Activator.CreateInstance(_writerDict[writerType], writerConfig);
                    var readerConfig = obj["sync"][i]["reader"].ToString();
                    var reader =
                        (IDataSyncReader) Activator.CreateInstance(_readerDict[readerType], readerConfig, writer);
                    syncs.Add(new Tuple<IDataSyncReader, IDataSyncWriter>(reader, writer));
                }
            }

            foreach (var sync in syncs)
            {
                await sync.Item1.RunAsync();
            }
            
            Console.WriteLine("Completed");
        }

        protected virtual Assembly[] GetGetAssemblies()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().Select(x => x.GetName().Name).ToList();

            var allAssemblies = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory)
                .Where(x => x.ToLower().EndsWith(".dll")).Select(x => Path.GetFileName(x).Replace(".dll", "")).ToList();
            var readerOrWriterAssemblies = allAssemblies.Where(
                x =>
                    (x.StartsWith("DataSync.") && x.EndsWith("Reader") ||
                     x.StartsWith("DataSync.") && x.EndsWith("Writer")) &&
                    (!loadedAssemblies.Contains(x))).ToList();
            var assemblies = readerOrWriterAssemblies
                .Select(x => Assembly.LoadFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{x}.dll")))
                .ToArray();
            return assemblies;
        }
    }
}