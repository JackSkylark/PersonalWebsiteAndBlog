using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.FileStorage.Serializers
{
    public interface IFileContentSerializer<T> where T : class
    {
        string Serialize(T obj);
        T Deserialize(string serializedObject, string key = "");
        string ExpectedFileExtension { get; }
    }
}
