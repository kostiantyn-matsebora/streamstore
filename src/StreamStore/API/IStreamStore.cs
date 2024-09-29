using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.API;

namespace StreamStore
{
    public interface IStreamStore
    {
        Task<IStream> OpenStreamAsync(string streamId, int expectedRevision, CancellationToken ct = default);
        Task DeleteAsync(string streamId, CancellationToken ct = default);
        Task<StreamEntity> GetAsync(string streamId, CancellationToken ct = default);
    }
}
