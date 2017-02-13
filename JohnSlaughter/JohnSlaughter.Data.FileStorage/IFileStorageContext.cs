using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JohnSlaughter.Data.FileStorage
{
    public interface IFileStorageContext<T> : IDisposable where T : class
    {
        Task Create(string key, T obj, CancellationToken cancellationToken);
        Task Update(string key, T obj, CancellationToken cancellationToken = default(CancellationToken));
        Task Delete(string key, CancellationToken cancellationToken = default(CancellationToken));
        Task<int> GetCount(CancellationToken cancellationToken = default(CancellationToken));
        Task<IDictionary<string, T>> Get(string key, CancellationToken cancellationToken = default(CancellationToken));        
        Task<IDictionary<string, T>> GetAll(CancellationToken cancellationToken = default(CancellationToken));
        Task<IDictionary<string, T>> GetPage(int pageNumber, int pageSize, CancellationToken cancellationToken = default(CancellationToken));
    }
}
