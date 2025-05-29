using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public interface IStreamStore
    {
        Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedRevision, CancellationToken cancellationToken = default);

        Task<IAsyncEnumerable<IStreamEventEnvelope>> BeginReadAsync(Id streamId, Revision fromRevision, CancellationToken cancellationToken = default);

        Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default);

        Task<IStreamMetadata> GetMetadataAsync(Id streamId, CancellationToken cancellationToken = default);
    }
}
