//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using StreamStore.S3.Client;
//using StreamStore.S3.Concurrency;
//using StreamStore.S3.Models;

//namespace StreamStore.S3.Operations
//{
//    internal sealed class S3StreamUploader
//    {
//        readonly IS3Client? client;
//        readonly S3StorageContext ctx;
 
//        S3StreamUploader(S3StorageContext ctx, IS3Client client)
//        {
//            this.client = client ?? throw new ArgumentNullException(nameof(client));
//            this.ctx = ctx ?? throw new ArgumentNullException(nameof(ctx));
//        }

//        public async Task UploadAsync(S3StreamMetadata metadata, IEnumerable<EventRecord> uncommited, CancellationToken token)
//        {
//            var tasks = uncommited.Select(async @event =>
//            {
//                var data = Converter.ToByteArray(@event);
//                var request = new UploadObjectRequest
//                {
//                    Key = ctx.EventKey(@event.Id),
//                    Data = data
//                };
//                await client!.UploadObjectAsync(request, token);
//            });

//            await Task.WhenAll(tasks);

//            var uploadRequest = new UploadObjectRequest
//            {
//                Key = ctx.MetadataKey,
//                Data = Converter.ToByteArray(new S3StreamMetadataRecord(metadata))
//            };

//            // Update stream
//            await client!.UploadObjectAsync(uploadRequest, token);
//        }

//        public static S3StreamUploader New(S3StorageContext ctx, IS3Client client)
//        {
//            return new S3StreamUploader(ctx, client);
//        }
//    }
//}
