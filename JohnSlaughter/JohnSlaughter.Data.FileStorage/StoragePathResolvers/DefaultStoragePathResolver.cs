using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.FileStorage.StoragePathResolvers
{
    public class DefaultStoragePathResolver : IStoragePathResolver
    {
        private IStoragePathOptions _pathOptions;

        public DefaultStoragePathResolver(IStoragePathOptions options)
        {
            _pathOptions = options;
        }

        public async Task<string> ResolvePath(
            string key = "", 
            string fileExtension = ".md", 
            bool ensureFoldersExist = false, 
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var folderPath = _pathOptions.RootDirectory 
                + _pathOptions.SubFolderName.Replace("/", _pathOptions.FolderSeparator);

            if (ensureFoldersExist && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                return folderPath + _pathOptions.FolderSeparator;
            }

            return Path.Combine(folderPath, key + fileExtension);
        }
    }
}
