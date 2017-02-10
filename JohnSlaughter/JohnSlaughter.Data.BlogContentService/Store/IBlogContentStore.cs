using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.BlogContentService.Store
{
    public interface IBlogContentStore
    {
        Task<List<string>> GetFileNames();
        Task<string> GetHtml(string path);
    }
}
