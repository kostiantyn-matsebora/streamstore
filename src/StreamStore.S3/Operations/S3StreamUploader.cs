using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Models;

namespace StreamStore.S3.Operations
{
    internal sealed class S3StreamUploader
    {
        readonly IS3Client? client;
        readonly S3StreamContext ctx;
 
        S3StreamUploader(S3StreamContext ctx, IS3Client client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task UploadAsync(S3StreamMetadata metadata, IEnumerable<EventRecord> uncommited, CancellationToken token)
        {
            await uncommited.ForEachAsync(5, async (@event) =>
            {
                var data = Converter.ToByteArray(@event);
                var request = new UploadObjectRequest
                {
                    Key = ctx.EventKey(@event.Id),
                    Data = data
                };
                await client!.UploadObjectAsync(request, token);
            });
           

            var uploadRequest = new UploadObjectRequest
            {
                Key = ctx.MetadataKey,
                Data = Converter.ToByteArray(new S3StreamMetadataRecord(metadata))
            };

            // Update stream
            await client!.UploadObjectAsync(uploadRequest, token);
        }

        public static S3StreamUploader New(S3StreamContext ctx, IS3Client client)
        {
            return new S3StreamUploader(ctx, client);
        }
    }
}
