using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.IO;
using JohnSlaughter.Data.FileStorage.Serializers;

namespace JohnSlaughter.Data.FileStorage
{
    public class FileStorageContext<T>: IFileStorageContext<T> where T : class
    {
        public FileStorageContext(
            ILogger<FileStorageContext<T>> logger,
            IFileContentSerializer<T> serializer,
            IStoragePathResolver pathResolver)
        {
            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            if (pathResolver == null) { throw new ArgumentNullException(nameof(pathResolver)); }
            if (serializer == null) { throw new ArgumentNullException(nameof(serializer)); }

            this.logger = logger;
            this.serializer = serializer;
            this.pathResolver = pathResolver;
        }

        protected IStoragePathResolver pathResolver;
        protected ILogger logger;
        protected IFileContentSerializer<T> serializer;

        // Create
        public virtual async Task Create(
            string key,
            T obj,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (obj == null) throw new ArgumentException("TObject obj must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFile = await pathResolver.ResolvePath(
                key,
                serializer.ExpectedFileExtension,
                true
                ).ConfigureAwait(false);

            if (File.Exists(pathToFile)) throw new InvalidOperationException("can't create file that already exists: " + pathToFile);

            var serialized = serializer.Serialize(obj);
            using (StreamWriter s = File.CreateText(pathToFile))
            {
                await s.WriteAsync(serialized);
            }
        }

        public virtual async Task Update(
            string key,
            T obj,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (obj == null) throw new ArgumentException("TObject obj must be provided");
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFile = await pathResolver.ResolvePath(
                key,
                serializer.ExpectedFileExtension,
                false).ConfigureAwait(false);

            if (!File.Exists(pathToFile)) throw new InvalidOperationException("can't update file that doesn't exist: " + pathToFile);
            //TODO: if instead of deleting the existing file
            // we just replace its contents then it opens the possibility
            // for custom queries based on file creation and last modified dates
            // whereas by deleting the file we lose the original creation date
            File.Delete(pathToFile); // delete the old version


            var serialized = serializer.Serialize(obj);
            using (StreamWriter s = File.CreateText(pathToFile))
            {
                await s.WriteAsync(serialized);
            }
        }

        public virtual async Task Delete(
            string key,
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFile = await pathResolver.ResolvePath(
                key,
                serializer.ExpectedFileExtension
                ).ConfigureAwait(false);

            if (!File.Exists(pathToFile)) throw new InvalidOperationException("can't delete item that does not exist: " + pathToFile);

            File.Delete(pathToFile);
        }

        public virtual async Task<IDictionary<string, T>> Get(string key, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("key must be provided");

            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFile = await pathResolver.ResolvePath(
                key,
                serializer.ExpectedFileExtension
                ).ConfigureAwait(false);

            if (!File.Exists(pathToFile)) return null;

            var obj = await LoadObject(pathToFile, key);

            return new Dictionary<string, T>
            {
                {key, obj}
            };
        }

        public virtual async Task<IDictionary<string, T>> GetAll(
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFolder = await pathResolver.ResolvePath(ensureFoldersExist: false).ConfigureAwait(false);
            
            var dict = new Dictionary<string, T>();

            if (!Directory.Exists(pathToFolder)) return dict;
            foreach (string file in Directory.EnumerateFiles(
                pathToFolder,
                "*" + serializer.ExpectedFileExtension,
                SearchOption.AllDirectories)
                )
            {
                var key = Path.GetFileNameWithoutExtension(file);
                var obj = await LoadObject(file, key);

                dict.Add(key, obj);
            }

            return dict;
        }

        public virtual async Task<IDictionary<string, T>> GetPage (int pageNumber = 1, int pageSize = 5, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFolder = await pathResolver.ResolvePath().ConfigureAwait(false);

            var list = new DirectoryInfo(pathToFolder)
                .EnumerateFiles()                
                .Select(x =>
                {
                    x.Refresh();
                    return x;
                })
                .OrderBy(x => x.CreationTimeUtc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await ConvertFileInfoArrayToObjectDictionary(list);
        }

        public virtual async Task<int> GetCount(
            CancellationToken cancellationToken = default(CancellationToken)
            )
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            var pathToFolder = await pathResolver.ResolvePath().ConfigureAwait(false);
            if (!Directory.Exists(pathToFolder)) return 0;

            var directory = new DirectoryInfo(pathToFolder);
            return directory.GetFileSystemInfos("*" + serializer.ExpectedFileExtension).Length;
        }

        protected async Task<T> LoadObject(string pathToFile, string key)
        {
            using (StreamReader reader = File.OpenText(pathToFile))
            {
                var payload = await reader.ReadToEndAsync();
                var result = serializer.Deserialize(payload, key);
                return result;
            }
        }

        private bool _disposed;

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }

        // Helper Methods

        private async Task<KeyValuePair<string, T>> ConvertFileInfoToObjectDictionary(FileInfo info)
        {
            var key = Path.GetFileNameWithoutExtension(info.Name);
            var obj = await LoadObject(info.ToString(), key);
            return new KeyValuePair<string, T>(key, obj);
        }

        private async Task<IDictionary<string, T>> ConvertFileInfoArrayToObjectDictionary(IEnumerable<FileInfo> infos)
        {
            var dictionary = new Dictionary<string, T>();

            foreach (var info in infos)
            {
                var key = Path.GetFileNameWithoutExtension(info.Name);
                var obj = await LoadObject(info.ToString(), key);
                dictionary.Add(key, obj);
            }

            return dictionary;
        }

    }
}
