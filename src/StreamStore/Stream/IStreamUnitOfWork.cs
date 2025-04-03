using System;
using System.Threading.Tasks;
using System.Threading;


namespace StreamStore
{
    public interface IStreamUnitOfWork: IDisposable
    {
        Task<IStreamUnitOfWork> AppendEventAsync(Id eventId, DateTime timestamp, object @event, CancellationToken cancellationToken = default);

        Task<Revision> CommitAsync(CancellationToken cancellationToken);
    }
}
