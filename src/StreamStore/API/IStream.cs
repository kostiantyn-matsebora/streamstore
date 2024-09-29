using System;
using System.Threading.Tasks;
using System.Threading;

namespace StreamStore.API
{
    public interface IStream : IDisposable
    {
        Task OpenAsync(string streamId, int expectedRevision, CancellationToken cancellationToken);
        IStream Add(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default);
        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
