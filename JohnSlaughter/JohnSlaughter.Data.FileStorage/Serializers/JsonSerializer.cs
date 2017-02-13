using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JohnSlaughter.Data.FileStorage.Serializers
{
    public class JsonSerializer<T> : IFileContentSerializer<T> where T : class
    {
        public string ExpectedFileExtension { get; } = ".json";

        public T Deserialize(string serializedObject, string key = "")
        {
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

        public string Serialize(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
