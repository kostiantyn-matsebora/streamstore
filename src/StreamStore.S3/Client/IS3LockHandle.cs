using System;
using System.Threading;
using System.Threading.Tasks;

namespace StreamStore.S3.Client
{
    public interface IS3LockHandle: IAsyncDisposable
    {
        Task ReleaseAsync(CancellationToken token);
    }
}