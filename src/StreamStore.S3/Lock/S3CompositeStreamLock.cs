using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;

namespace StreamStore.S3.Lock
{
    internal class S3CompositeStreamLock : IS3StreamLock
    {
        readonly IS3StreamLock[] locks;

        public S3CompositeStreamLock(params IS3StreamLock[] locks)
        {
            this.locks = locks ?? throw new ArgumentNullException(nameof(locks));
        }

        public async Task<IS3LockHandle?> AcquireAsync(CancellationToken token)
        {
            var acquiredHandles = new List<IS3LockHandle>();
            foreach (var @lock in locks)
            {
                var handle = await @lock.AcquireAsync(token);
                if (handle == null)
                {
                    foreach (var acquiredHandle in acquiredHandles)
                    {
                        await acquiredHandle.ReleaseAsync(token);
                    }
                    return null;
                }
                acquiredHandles.Add(handle);
            }

            return new S3CompositeLockHandle(acquiredHandles.ToArray());
        }
    }
}
