//using System;
//using System.Collections.Generic;
//using System.Text;
//using StreamStore.S3.Models;
//using System.Threading.Tasks;
//using System.Threading;
//using StreamStore.S3.Client;
//using StreamStore.S3.Concurrency;

//namespace StreamStore.S3.Operations
//{
//    internal class S3StreamMetadataLoader
//    {
//        readonly IS3Client? client;
//        readonly S3StorageContext ctx;

//        S3StreamMetadataLoader(S3StorageContext container, IS3Client client)
//        {
//            this.client = client ?? throw new ArgumentNullException(nameof(client));
//            this.ctx = container ?? throw new ArgumentNullException(nameof(container));
//        }

//        public async Task<S3StreamMetadata?> LoadAsync(CancellationToken token) //TODO: Add retry logic
//        {
//            var response = await client!.FindObjectAsync(ctx.MetadataKey, token);

//            if (response == null) return null; // Probably already has been deleted

//            var metadataRecord = Converter.FromByteArray<S3StreamMetadataRecord>(response.Data!);
//            return  metadataRecord!.ToMetadata();
//        }

//        public static S3StreamMetadataLoader New(S3StorageContext ctx, IS3Client client)
//        {
//            return new S3StreamMetadataLoader(ctx, client);
//        }
//    }
//}
