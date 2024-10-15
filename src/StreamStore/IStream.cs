using System;
using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public interface IStream : IDisposable
    {
        Task OpenAsync(Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default);
        Task<IStream> AddAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default);
        Task<Revision> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
