using System;
using System.Threading.Tasks;
using System.Threading;


namespace StreamStore
{
    public interface IWriteOnlyStream: IDisposable
    {
        Task<IWriteOnlyStream> AddAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default);

        Task<Revision> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
