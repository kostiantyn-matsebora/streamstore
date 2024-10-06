using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StreamStore.S3.Client;
using StreamStore.S3.Models;

namespace StreamStore.S3.Operations
{
    internal sealed class S3StreamLoader : IDisposable
    {
        IS3Client? client;
        Id streamId;
        public S3StreamLoader(Id streamId, IS3Client client)
        {
            if (streamId == Id.None)
                throw new ArgumentException("StreamId cannot be empty", nameof(streamId));
            this.streamId = streamId;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task<S3Stream?> LoadAsync(CancellationToken token)
        {

            var response = await client!.FindObjectAsync(S3Naming.StreamKey(streamId), token);

            if (response == null) return null;// Probably already has been deleted


            var metadata = Converter.FromByteArray<S3StreamMetadata>(response.Data!);
            var events = new S3EventRecordCollection();

            foreach (var eventMetadata in metadata!.Events!)
            {
                var eventResponse = await client.FindObjectAsync(S3Naming.EventKey(streamId, eventMetadata.Id), token);
                var @event = Converter.FromByteArray<S3EventRecord>(eventResponse!.Data!);
                events.Add(@event!);
            }

            return S3Stream.New(metadata, events);
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
    }
}
