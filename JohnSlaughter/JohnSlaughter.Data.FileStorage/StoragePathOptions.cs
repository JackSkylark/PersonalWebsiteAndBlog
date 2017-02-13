using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.FileStorage
{
    public class StoragePathOptions : IStoragePathOptions
    {
        public string RootDirectory { get; set; }
        public string SubFolderName { get; set; } = "/file_storage";

        public string FolderSeparator
        {
            get { return Path.DirectorySeparatorChar.ToString(); }
        }
    }
}
