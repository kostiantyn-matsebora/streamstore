using System.Threading;
using System.Threading.Tasks;

namespace StreamStore
{
    public interface IStreamStore
    {
        Task<IStream> OpenAsync(Id streamId, CancellationToken cancellationToken = default);
        Task DeleteAsync(Id streamId, CancellationToken cancellationToken = default);
    }
}
