using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public interface IStreamStore
    {
        Task<IStreamUnitOfWork> BeginWriteAsync(Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default);

        Task<IAsyncEnumerable<IStreamEvent>> BeginReadAsync(Id streamId, Revision startFrom, CancellationToken cancellationToken = default);

        Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default);
    }
}
