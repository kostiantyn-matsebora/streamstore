using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public interface IStreamReader
    {
        Task<EventRecord[]> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default);
    }
}
