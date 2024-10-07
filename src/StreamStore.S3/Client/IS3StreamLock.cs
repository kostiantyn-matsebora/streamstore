using System;
using System.Threading.Tasks;
using System.Threading;

namespace StreamStore.S3.Client
{
    public interface IS3StreamLock
    {
        abstract Task<IS3LockHandle?> AcquireAsync(CancellationToken token);
    }
}