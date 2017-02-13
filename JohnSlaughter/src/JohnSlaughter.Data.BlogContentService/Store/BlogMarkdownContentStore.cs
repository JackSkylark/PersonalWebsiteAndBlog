using JohnSlaughter.Data.Utilities.Exceptions;
using Markdig;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.BlogContentService.Store
{
    public class BlogMarkdownContentStore : IBlogContentStore
    {
        private string _contentDirectoryPath;

        public BlogMarkdownContentStore(string contentDirectoryPath)
        {
            contentDirectoryPath = contentDirectoryPath.Trim();

            if (!Directory.Exists(contentDirectoryPath))
            {
                throw new DirectoryNotFoundException(contentDirectoryPath);
            };

            _contentDirectoryPath = contentDirectoryPath;
        }

        public async Task<List<string>> GetFileNames()
        {
            var files = Directory.GetFiles(_contentDirectoryPath);
            var fileNames = files.Select(Path.GetFileName).Where(x => Path.GetExtension(x) == ".md").ToList(); // Get Markdown Files
            return fileNames.ToList();
        }

        public async Task<string> GetHtml(string fileName)
        {
            var fileContent = await ReadFile(fileName);
            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            return Markdown.ToHtml(fileContent, pipeline);
        }

        private async Task<string> ReadFile(string fileName)
        {
            var filePath = Path.Combine(_contentDirectoryPath, fileName);

            if (!File.Exists(filePath))
            {
                throw new EntityNotFoundException(filePath);
            }

            using (var reader = File.OpenText(filePath))
            {
                var fileText = await reader.ReadToEndAsync();
                return fileText;
            }
        }
    }
}
