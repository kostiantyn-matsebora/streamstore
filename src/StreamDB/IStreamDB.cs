using System.Threading;
using System.Threading.Tasks;
using StreamDB.Contracts;

namespace StreamDB
{
    public interface IStreamDB
    {
        Task SaveAsync(string streamId, EventEnvelope[] uncommited, int expectedRevision, CancellationToken ct = default);
        Task DeleteAsync(string streamId, CancellationToken ct = default);
        Task<Stream> GetAsync(string streamId, CancellationToken ct = default);
    }
}
