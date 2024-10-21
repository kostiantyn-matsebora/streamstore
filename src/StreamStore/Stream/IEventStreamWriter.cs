using System;
using System.Threading.Tasks;
using System.Threading;


namespace StreamStore
{
    public interface IEventStreamWriter: IDisposable
    {
        Task<IEventStreamWriter> AppendAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default);

        Task<Revision> CommitAsync(CancellationToken cancellationToken);
    }
}
