using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using StreamStore.S3.Client;
using StreamStore.S3.Models;

namespace StreamStore.S3.Operations
{
    internal sealed class S3StreamLoader
    {
        readonly IS3Client? client;
        readonly S3StreamContext ctx;
        
        S3StreamLoader(S3StreamContext ctx, IS3Client client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
        }


        public async Task<S3Stream?> LoadAsync(CancellationToken token) //TODO: Add retry logic
        {

            var metadata = await S3StreamMetadataLoader
                    .New(ctx, client!)
                    .LoadAsync(token);

            if (metadata == null) return null;

            var events = new EventRecordCollection();

            foreach (var eventMetadata in metadata)
            {
                events.Add(await GetEvent(eventMetadata, token));
            }

            return S3Stream.New(metadata, events);
        }

        async Task<EventRecord> GetEvent(S3EventMetadata eventMetadata, CancellationToken token)
        {
            var eventResponse = await client!.FindObjectAsync(ctx.EventKey(eventMetadata.Id), token);
            var eventRecord = Converter.FromByteArray<EventRecord>(eventResponse!.Data!);
            return eventRecord!;
        }
        public static S3StreamLoader New(S3StreamContext ctx, IS3Client client)
        {
            return new S3StreamLoader(ctx, client);
        }
    }
}
