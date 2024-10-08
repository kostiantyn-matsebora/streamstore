using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Concurrency;


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
            // Delete all objects from container
            List<ObjectDescriptor> victims;

            // Delete events
            do
            {
                var response = await client!.ListObjectsAsync(ctx.EventsKey, null, token);

                if (response == null)
                    break;

                victims = response.Objects!.ToList();
                var tasks = victims.Select(async e =>
                {
                    await client.DeleteObjectByFileIdAsync(e.FileId!, e.FileName!, token);
                });

                await Task.WhenAll(tasks);

            } while (victims.Any());


            // Delete metadata
            var metadata = await client!.FindObjectDescriptorAsync(ctx.MetadataKey, token);
            if (metadata == null) return;

            await client!.DeleteObjectByFileIdAsync(metadata.FileId!, metadata.FileName!, token);
        }

        public static S3StreamDeleter New(S3StreamContext ctx, IS3Client client)
        {
            return new S3StreamDeleter(ctx, client);
        }
    }
}
