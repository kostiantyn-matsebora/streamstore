using System.Threading;
using System.Threading.Tasks;


namespace StreamDB
{
    public interface IStreamDB
    {
        Task AppendAsync(string streamId, IUncommitedEvent[] uncommited, int expectedRevision, CancellationToken ct = default);
        Task DeleteAsync(string streamId, CancellationToken ct = default);
        Task<IStreamEntity> GetAsync(string streamId, CancellationToken ct = default);
    }
}
