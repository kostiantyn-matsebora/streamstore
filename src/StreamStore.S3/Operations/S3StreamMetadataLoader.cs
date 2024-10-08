using System;
using System.Collections.Generic;
using System.Text;
using StreamStore.S3.Models;
using System.Threading.Tasks;
using System.Threading;
using StreamStore.S3.Client;

namespace StreamStore.S3.Operations
{
    internal class S3StreamMetadataLoader
    {
        readonly IS3Client? client;
        readonly S3StreamContext ctx;

        S3StreamMetadataLoader(S3StreamContext ctx, IS3Client client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }

        public async Task<S3StreamMetadata?> LoadAsync(CancellationToken token) //TODO: Add retry logic
        {
            var response = await client!.FindObjectAsync(ctx.MetadataKey, token);

            if (response == null) return null; // Probably already has been deleted

            var metadataRecord = Converter.FromByteArray<S3StreamMetadataRecord>(response.Data!);
            return  metadataRecord!.ToMetadata();
        }

        public static S3StreamMetadataLoader New(S3StreamContext ctx, IS3Client client)
        {
            return new S3StreamMetadataLoader(ctx, client);
        }
    }
}
