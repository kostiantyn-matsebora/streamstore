using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Lock
{
    class S3FileLockHandle : IS3LockHandle
    {
        bool lockReleased = false;
        Id streamId;
        string? fileId;
        readonly IS3Factory? factory;

        public S3FileLockHandle(Id streamId, string fileId, IS3Factory factory)
        {
            if (streamId == Id.None)
                throw new ArgumentException("Stream id is not set.", nameof(streamId));

            this.streamId = streamId;
            this.fileId = fileId;
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task ReleaseAsync(CancellationToken token)
        {
            if (!lockReleased)
            {
                lockReleased = true;
                await using var client = factory!.CreateClient();
                await client!.DeleteObjectByFileIdAsync(fileId!, S3Naming.LockKey(streamId), token);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        async ValueTask DisposeAsync(bool disposing)
        {
            if (disposing)
            {
                await ReleaseAsync(CancellationToken.None);
                fileId = null;
                streamId = Id.None;
            }
        }
    }
}
