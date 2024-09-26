using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace StreamDB
{
    public interface IEventStore
    {
        Task<StreamEntity> FindAsync(string streamId, CancellationToken cancellationToken);
        Task InsertAsync(string streamId, IEnumerable<EventEntity> events, CancellationToken cancellationToken);
        Task DeleteAsync(string streamId, CancellationToken cancellationToken);
    }
}
