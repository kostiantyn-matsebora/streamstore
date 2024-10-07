﻿using System;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Models;

namespace StreamStore.S3.Operations
{
    internal sealed class S3StreamUpdater
    {
        readonly IS3Client? client;
        readonly S3Stream? stream;

        S3StreamUpdater(S3Stream stream, IS3Client client)
        {
            this.stream = stream ?? throw new ArgumentNullException(nameof(stream));
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }


        public async Task UpdateAsync(CancellationToken token)
        {
            var streamId = stream!.Metadata.StreamId!;
            UploadObjectRequest request; 

            foreach (var @event in stream.Events)
            {
                var data = Converter.ToByteArray(@event);

                request = new UploadObjectRequest
                {
                    Key = S3Naming.EventKey(streamId, @event.Id),
                    Data = data
                };
                await client!.UploadObjectAsync(request, token);
            }
           

            request = new UploadObjectRequest
            {
                Key = S3Naming.StreamMetadataKey(streamId),
                Data = Converter.ToByteArray(new S3StreamMetadataRecord(stream.Metadata))
            };

            // Update stream
            await client!.UploadObjectAsync(request, token);
        }

        public static S3StreamUpdater New(S3Stream stream, IS3Client client)
        {
            return new S3StreamUpdater(stream, client);
        }
    }
}