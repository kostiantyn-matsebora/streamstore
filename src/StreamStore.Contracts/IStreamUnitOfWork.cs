using System.Threading.Tasks;
using System.Threading;
using System;

namespace StreamStore
{
    public interface IStreamUnitOfWork: IDisposable
    {
        Task<IStreamUnitOfWork> AddAsync(Id eventId, DateTime timestamp, string data, CancellationToken token = default);
        Task<Revision> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
