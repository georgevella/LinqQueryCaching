using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Briefs.DataLayer.Caching
{
    internal class CacheItemSerializationHelper
    {
        private static readonly JsonSerializer Serializer = JsonSerializer.Create(new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
            Binder = new EfProxySerializationBinder(),
        });

        public static byte[] Serialize<T>(List<T> listOfT)
        {
            using (var buffer = new MemoryStream())
            using (var writer = new StreamWriter(buffer)
            {
                AutoFlush = true
            })
            {
                Serializer.Serialize(writer, listOfT, typeof(List<T>));

                using (var file = File.OpenWrite(string.Format("E:\\Temp\\brieflet-cache\\{0}.json", Stopwatch.GetTimestamp())))
                {
                    buffer.Position = 0;
                    buffer.CopyTo(file);
                    file.Flush();
                }


                return buffer.GetBuffer();
            }
        }

        public static List<T> Deserialize<T>(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer))
            using (var reader = new StreamReader(stream))
            {
                return Serializer.Deserialize<List<T>>(new JsonTextReader(reader));
            }
        }

        private class EfProxySerializationBinder : DefaultSerializationBinder
        {
            private readonly List<Type> _typeStore;

            public EfProxySerializationBinder()
            {
                var typeList = GetType().Assembly.GetTypes();

                _typeStore = typeList.Where(t => t.IsPublic && !t.IsNested && t.IsClass && !t.IsInterface && !t.IsGenericType && !t.Name.EndsWith("Attribute") && !t.FullName.Contains("Annotations") && !t.FullName.Contains("Migrations")).OrderBy(x => x.Name).ToList();

            }

            public override Type BindToType(string assemblyName, string typeName)
            {
                if (assemblyName.StartsWith("EntityFrameworkDynamicProxies"))
                {
                    typeName = typeName.Substring("System.Data.Entity.DynamicProxies.".Length);
                    typeName = typeName.Substring(0, typeName.IndexOf('_'));

                    assemblyName = assemblyName.Substring("EntityFrameworkDynamicProxies-".Length);

                    var type = _typeStore.FirstOrDefault(t => t.Name.StartsWith(typeName));
                    if (type != null)
                    {
                        assemblyName = type.Assembly.FullName;
                        typeName = type.FullName;
                    }
                }
                return base.BindToType(assemblyName, typeName);
            }
        }

    }
}