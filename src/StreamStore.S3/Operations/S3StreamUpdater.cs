using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StreamStore.S3.Client;
using StreamStore.S3.Models;

namespace StreamStore.S3.Operations
{
    internal sealed class S3StreamUpdater : IDisposable
    {
        IS3Client? client;
        S3Stream? stream;

        public S3StreamUpdater(S3Stream stream, IS3Client client)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task UpdateAsync(CancellationToken token)
        {
            var streamId = stream!.Metadata.StreamId!;
            foreach (var @event in stream.Events)
            {
                var data = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                await client!.UploadObjectAsync(
                    S3Naming.EventKey(streamId, @event.Id),
                    Converter.ToByteArray(data),
                    S3EventMetadataCollection.New(@event.Id, @event.Revision),
                    token);
            }

            await client!.UploadObjectAsync(
                S3Naming.StreamKey(streamId),
                Converter.ToByteArray(stream.Metadata),
                S3MetadataCollection.Empty,
                token);
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
                stream = null;
            }
        }
    }
}
