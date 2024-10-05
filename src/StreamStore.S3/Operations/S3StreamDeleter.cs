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
            var data = await client!.FindObjectAsync(S3Naming.StreamKey(streamId), token);

            if (data == null)
                return;

            var metadata = Converter.FromString<S3StreamMetadata>(data);

            // Delete all events
            foreach (var eventMetadata in metadata!.Events!)
            {
                await client!.DeleteObjectAsync(S3Naming.EventKey(streamId, eventMetadata.Id), token);
            }

            // Delete metadata
            await client!.DeleteObjectAsync(S3Naming.StreamKey(streamId), token);
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
                client?.Dispose();
                client = null;
                streamId = Id.None;
            }
        }
    }
}
