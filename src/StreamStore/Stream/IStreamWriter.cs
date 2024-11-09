using System;
using System.Threading.Tasks;
using System.Threading;


namespace StreamStore
{
    public interface IStreamWriter: IDisposable
    {
        Task<IStreamWriter> AppendEventAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default);

        Task<Revision> CommitAsync(CancellationToken cancellationToken);
    }
}
