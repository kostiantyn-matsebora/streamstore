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
        IS3Client? client;

        public S3FileLockHandle(Id streamId, string fileId, IS3Client client)
        {
            if (streamId == Id.None)
                throw new ArgumentException("Stream id is not set.", nameof(streamId));

            this.streamId = streamId;
            this.fileId = fileId;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task ReleaseAsync(CancellationToken token)
        {
            if (!lockReleased)
            {
                lockReleased = true;
                await client!.DeleteObjectAsync(S3Naming.LockKey(streamId), token, fileId);
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
                client = null;
                fileId = null;
                streamId = Id.None;
            }
        }
    }
}
