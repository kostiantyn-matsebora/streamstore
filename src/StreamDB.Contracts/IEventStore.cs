using System.Threading.Tasks;
using System.Threading;

namespace StreamDB
{
    public interface IEventStore
    {
        Task<StreamData> FindAsync(string streamId, CancellationToken cancellationToken);
        Task InsertAsync(string streamId, EventData[] events, CancellationToken cancellationToken);
        Task DeleteAsync(string streamId, CancellationToken cancellationToken);
    }
}
