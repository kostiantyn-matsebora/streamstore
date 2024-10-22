using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public interface IStreamDatabase
    {
        Task<StreamRecord?> FindAsync(Id streamId, CancellationToken token = default);
        Task<StreamMetadataRecord?> FindMetadataAsync(Id streamId, CancellationToken token = default);
        Task DeleteAsync(Id streamId, CancellationToken token = default);
        Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default);
        Task<EventRecord[]> ReadAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default);
    }
}
