using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Lock
{
    partial class S3FileLock : IS3StreamLock
    {
        private readonly IS3TransactionContext ctx;
        readonly IS3Factory factory;

        public S3FileLock(IS3TransactionContext ctx, IS3Factory factory)
        {
            this.ctx = ctx ?? throw new ArgumentNullException(nameof(ctx)); 

            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public async Task<IS3LockHandle?> AcquireAsync(CancellationToken token)
        {
            // Trying to figure out if lock already acquired
            await using var client = factory.CreateClient();
            var response = await client.FindObjectAsync(ctx.LockKey, token);
            if (response != null) return null;

            // Upload lock
            var request = new UploadObjectRequest
            {
                Key = ctx.LockKey,
                Data = Converter.ToByteArray(new LockId(ctx.TransactionId)),
            };

            var uploadResponse = await client.UploadObjectAsync(request, token);
            if (uploadResponse == null) return null;

            // Trying to figure out if lock already acquired by someone else
            var result = await client.FindObjectAsync(ctx.LockKey, token);
            if (result == null) return null;

            var existingLockId = Converter.FromByteArray<LockId>(result!.Data!);

            if (existingLockId == null) return null;

            if (!existingLockId!.Equals(ctx.TransactionId)) return null;

            return new S3FileLockHandle(ctx, uploadResponse!.FileId!, factory);
        }
    }
}
