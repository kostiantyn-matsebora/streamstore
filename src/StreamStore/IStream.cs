using System;
using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public interface IStream : IDisposable
    {
        Task OpenAsync(Id streamId, int expectedRevision, CancellationToken cancellationToken = default);
        IStream Add(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
