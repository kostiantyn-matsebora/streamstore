using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;


namespace StreamStore.S3.Operations
{
    internal sealed class S3StreamDeleter
    {
        readonly IS3Client? client;
        readonly S3StreamContext ctx;

        public S3StreamDeleter(S3StreamContext ctx, IS3Client client)
        {
            this.ctx = ctx;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task DeleteAsync(CancellationToken token)
        {
            //Delete all objects from container
            await client!.DeleteObjectAsync(ctx.EventsKey, null, token);
            await client!.DeleteObjectAsync(ctx.MetadataKey, null, token);
        }

        public static S3StreamDeleter New(S3StreamContext ctx, IS3Client client)
        {
            return new S3StreamDeleter(ctx, client);
        }
    }
}
