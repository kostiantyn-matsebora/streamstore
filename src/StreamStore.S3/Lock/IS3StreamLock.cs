using System;
using System.Threading.Tasks;
using System.Threading;

namespace StreamStore.S3.Lock
{
    public interface IS3StreamLock: IDisposable
    {
        abstract Task<bool> AcquireAsync(CancellationToken token);
        abstract Task ReleaseAsync(CancellationToken token);
    }
}