using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;

namespace StreamStore
{
    public interface IEventTable
    {
        Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken);
        Task<StreamMetadataRecord?> FindMetadataAsync(string streamId, CancellationToken cancellationToken);
        Task InsertAsync(string streamId, IEnumerable<EventRecord> uncommited, CancellationToken cancellationToken);
        Task DeleteAsync(string streamId, CancellationToken cancellationToken);
    }
}
