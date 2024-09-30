using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public interface IStreamStore
    {
        Task<IStream> OpenStreamAsync(Id streamId, CancellationToken ct = default);
        Task<IStream> OpenStreamAsync(Id streamId, int expectedRevision, CancellationToken ct = default);
        Task DeleteAsync(Id streamId, CancellationToken ct = default);
        Task<StreamEntity> GetAsync(Id streamId, CancellationToken ct = default);
    }
}
