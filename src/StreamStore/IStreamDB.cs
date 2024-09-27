using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore
{
    public interface IStreamDB
    {
        Task AppendAsync(string streamId, IEnumerable<UncommitedEvent> uncommited, int expectedRevision, CancellationToken ct = default);
        Task DeleteAsync(string streamId, CancellationToken ct = default);
        Task<StreamEntity> GetAsync(string streamId, CancellationToken ct = default);
    }
}
