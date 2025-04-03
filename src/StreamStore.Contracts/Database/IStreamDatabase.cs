using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public interface IStreamDatabase: IStreamReader
    {
        Task<Revision?> GetActualRevision(Id streamId, CancellationToken token = default);
        Task DeleteAsync(Id streamId, CancellationToken token = default);
        Task<IStreamWriter> BeginAppendAsync(Id streamId, Revision expectedStreamVersion, CancellationToken token = default);
    }
}
