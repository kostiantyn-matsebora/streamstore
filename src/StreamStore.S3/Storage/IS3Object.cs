using System.Threading;
using System.Threading.Tasks;


namespace StreamStore.S3.Storage
{
    interface IS3Object
    {
        S3ObjectState State { get; }
        Task UploadAsync(CancellationToken token);
        Task LoadAsync(CancellationToken token);
        Task DeleteAsync(CancellationToken token);
    }
}
