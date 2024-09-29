using System.Threading.Tasks;
using System.Threading;

namespace StreamStore
{
    public interface IEventDatabase
    {
        Task<StreamRecord?> FindAsync(string streamId, CancellationToken cancellationToken);
        Task<StreamMetadataRecord?> FindMetadataAsync(string streamId, CancellationToken cancellationToken);
        Task DeleteAsync(string streamId, CancellationToken cancellationToken);

        IEventUnitOfWork CreateUnitOfWork(string streamId, int expectedStreamVersion);
    }
}
