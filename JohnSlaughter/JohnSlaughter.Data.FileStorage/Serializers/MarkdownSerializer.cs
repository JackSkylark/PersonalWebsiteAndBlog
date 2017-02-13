using JohnSlaughter.Data.FileStorage.Models;
using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.FileStorage.Serializers
{
    public class MarkdownSerializer<T> : IFileContentSerializer<string>
    {
        public string ExpectedFileExtension { get; } = ".md";

        public string Deserialize(string serializedObject, string key = "")
        {
            return Markdown.ToHtml(serializedObject, new MarkdownPipelineBuilder().UseAdvancedExtensions().Build());
        }

        public string Serialize(string obj)
        {
            throw new NotImplementedException();
        }
    }

    
}
