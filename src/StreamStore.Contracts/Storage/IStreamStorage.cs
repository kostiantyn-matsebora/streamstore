using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public interface IStreamStorage: IStreamReader, IStreamWriter
    {
        Task<IStreamMetadata?> GetMetadata(Id streamId, CancellationToken token = default);
        Task DeleteAsync(Id streamId, CancellationToken token = default);
    }
}
