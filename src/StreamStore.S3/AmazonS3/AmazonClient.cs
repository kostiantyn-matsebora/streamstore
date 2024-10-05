using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using StreamStore.S3.Client;


namespace StreamStore.S3.AmazonS3
{
    internal sealed class AmazonClient : IS3Client
    {
        AmazonS3Client? client;
        string? bucketName;

        public AmazonClient(AmazonS3Client client, string bucketName)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
        }

        public async Task DeleteObjectAsync(string key, CancellationToken token)
        {          
            await client!.DeleteObjectAsync(bucketName, key, token);
        }

        public async Task<IEnumerable<byte[]>?> FindObjectsByPrefixAsync(string prefix, CancellationToken token)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = prefix,
            };

            var listObjectsV2Paginator = client!.Paginators.ListObjectsV2(new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = prefix
            });

            var objects = new List<byte[]>();

            await foreach (var response in listObjectsV2Paginator.Responses)
            {
                response
                    .S3Objects
                    .ForEach(async entry => objects.Add(await GetObjectData(objects, entry, token)));
            }

            return objects.Any() ? objects: null;
        }

        async Task<byte[]> GetObjectData(List<byte[]> objects, S3Object entry, CancellationToken token)
        {
            var objectResponse = await client!.GetObjectAsync(bucketName, entry.Key, token);
            var memory = new Memory<byte>();
            await objectResponse.ResponseStream.WriteAsync(memory, token);
            return memory.ToArray();
        }

        public async Task UploadObjectAsync(string key, byte[] data, IS3ReadonlyMetadataCollection metadata, CancellationToken token, bool lockObject = false)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = new MemoryStream(data),
                AutoCloseStream = true,
                AutoResetStreamPosition = true,
            };

            if (lockObject)
                request.ObjectLockLegalHoldStatus = ObjectLockLegalHoldStatus.On;

            metadata.Keys
                .ToList()
                .ForEach(k => request.Metadata.Add(k, metadata[k]));

            var response = await client!.PutObjectAsync(request, token);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new AmazonS3Exception($"Failed to put object to S3. HttpStatusCode: {response.HttpStatusCode}.");
        }

        public async Task<byte[]?> FindObjectAsync(string key, CancellationToken token)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key,
            };

            var response = await client!.GetObjectAsync(request, token);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return null;
            var memory = new Memory<byte>();
            response.ResponseStream.Write(memory.Span);
            return memory.ToArray();
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
                client!.Dispose();
                client = null;
                bucketName = null;
            }
        }
    }
}
