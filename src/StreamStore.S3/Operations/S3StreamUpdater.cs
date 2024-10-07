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

        S3StreamUpdater(S3Stream stream, IS3Client client)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task UpdateAsync(CancellationToken token)
        {
            var streamId = stream!.Metadata.StreamId!;

            Parallel.ForEach(stream.Events, async @event =>
            {
                var data = Converter.ToByteArray(@event);

                var request = new UploadObjectRequest
                {
                    Key = S3Naming.EventKey(streamId, @event.Id),
                    Data = data
                };
                await client!.UploadObjectAsync(request,token);
            });

            var request = new UploadObjectRequest
            {
                Key = S3Naming.StreamMetadataKey(streamId),
                Data = Converter.ToByteArray(new S3StreamMetadataRecord(stream.Metadata))
            };

            // Update stream
            await client!.UploadObjectAsync(request, token);
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
                stream = null;
            }
        }

        public static S3StreamUpdater New(S3Stream stream, IS3Client client)
        {
            return new S3StreamUpdater(stream, client);
        }
    }
}
