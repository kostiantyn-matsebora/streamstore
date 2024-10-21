using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public interface IStreamStore
    {
        Task<IStream> OpenStreamAsync(Id streamId, CancellationToken cancellationToken = default);
        Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default);
    }
}
