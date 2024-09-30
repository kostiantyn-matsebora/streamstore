using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public interface IStreamDatabase
    {
        Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken);
        Task<StreamMetadataRecord?> FindMetadataAsync(string streamId, CancellationToken cancellationToken);
        Task DeleteAsync(string streamId, CancellationToken cancellationToken);
        IStreamUnitOfWork BeginAppend(string streamId, int expectedStreamVersion = 0);
    }
}
