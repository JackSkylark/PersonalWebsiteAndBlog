using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace JohnSlaughter.Data.FileStorage.Serializers
{
    public class YamlSerializer<T> : IFileContentSerializer<T> where T : class
    {
        public string ExpectedFileExtension { get; } = ".yaml";

        public T Deserialize(string serializedObject, string key = "")
        {
            var yamlDeserializer = new Deserializer();
            return yamlDeserializer.Deserialize<T>(serializedObject);
        }

        public string Serialize(T obj)
        {
            var yamlSerializer = new Serializer();
            return yamlSerializer.Serialize(obj);
        }
    }
}
