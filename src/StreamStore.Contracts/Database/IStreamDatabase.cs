using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public interface IStreamDatabase: IStreamReader
    {
        Task<EventMetadataRecordCollection?> FindMetadataAsync(Id streamId, CancellationToken token = default);
        Task DeleteAsync(Id streamId, CancellationToken token = default);
        Task<IStreamUnitOfWork> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default);
    }
}
