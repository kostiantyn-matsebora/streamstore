using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.S3.Storage
{
    internal interface IS3TransactionalStorage
    {
        Task<S3MetadataObject> LoadPersistentMetadataAsync(Id streamId, CancellationToken token = default);
        Task DeletePersistentContainerAsync(Id streamId, CancellationToken token = default);
        Task<S3StreamContainer> LoadPersistentContainerAsync(Id streamId, Revision startFrom, int count, CancellationToken token = default);
        S3StreamContainer GetPersistentContainer(string name);
        S3StreamContainer GetTransientContainer(string name);
        S3LockObject GetLock(string name);
    }
}
