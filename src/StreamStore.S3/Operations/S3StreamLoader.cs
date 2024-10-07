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
        readonly Id streamId;
        public S3StreamLoader(Id streamId, IS3Client client)
        {
            if (streamId == Id.None)
                throw new ArgumentException("StreamId cannot be empty", nameof(streamId));
            this.streamId = streamId;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task<S3Stream?> LoadAsync(CancellationToken token) //TODO: Add retry logic
        {
            var response = await client!.FindObjectAsync(S3Naming.StreamMetadataKey(streamId), token);

            if (response == null) return null; // Probably already has been deleted

            var metadataRecord = Converter.FromByteArray<S3StreamMetadataRecord>(response.Data!);
            var metadata = metadataRecord!.ToStreamMetadata();

            var events = new S3EventRecordCollection();

            foreach (var eventMetadata in metadata)
            {
                events.Add(await GetEvent(eventMetadata, token));
            }

            return S3Stream.New(metadata, events);
        }

        private async Task<S3EventRecord> GetEvent(S3EventMetadata eventMetadata, CancellationToken token)
        {
            var eventResponse = await client!.FindObjectAsync(S3Naming.EventKey(streamId, eventMetadata.Id), token);
            var eventRecord = Converter.FromByteArray<S3EventRecord>(eventResponse!.Data!);
            return eventRecord!;
        }
    }
}
