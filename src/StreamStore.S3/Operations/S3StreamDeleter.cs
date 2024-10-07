using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Models;

namespace StreamStore.S3.Operations
{
    internal sealed class S3StreamDeleter : IDisposable
    {
        IS3Client? client;
        Id streamId;

        public S3StreamDeleter(Id streamId, IS3Client client)
        {
            if (streamId == Id.None)
                throw new ArgumentException("StreamId cannot be empty", nameof(streamId));
            this.streamId = streamId;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task DeleteAsync(CancellationToken token)
        {

            // First get stream metadata
            var response = await client!.FindObjectAsync(S3Naming.StreamMetadataKey(streamId), token);
            if (response == null) return;

            var metadata = Converter.FromByteArray<S3StreamMetadataRecord>(response.Data!);

            // Delete all events
            foreach (var eventMetadata in metadata!.Events!)
                await client!.DeleteObjectAsync(S3Naming.StreamPrefix(streamId), S3Naming.EventKey(streamId, eventMetadata.Id), token);

            // Delete metadata
            await client!.DeleteObjectAsync(S3Naming.StreamPrefix(streamId),S3Naming.StreamMetadataKey(streamId), token);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        void Dispose(bool disposing)
        {
            if (disposing)
            {
                client?.DisposeAsync().ConfigureAwait(false);
                client = null;
                streamId = Id.None;
            }
        }

        public static S3StreamDeleter New(Id streamId, IS3Client client)
        {
            return new S3StreamDeleter(streamId, client);
        }
    }
}
