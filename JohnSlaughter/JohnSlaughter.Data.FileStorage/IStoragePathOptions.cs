using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.FileStorage
{
    public interface IStoragePathOptions
    {
        string RootDirectory { get; }
        string SubFolderName { get; }
        string FolderSeparator { get; }
    }
}
