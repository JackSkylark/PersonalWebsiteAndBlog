using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.FileStorage
{
    public interface IStoragePathResolver
    {
        Task<string> ResolvePath(
            string key = "",
            string fileExtension = ".md",
            bool ensureFoldersExist = false,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
