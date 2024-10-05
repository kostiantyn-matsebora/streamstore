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
    internal class AmazonClient : IS3Client
    {
        bool disposedValue;
        AmazonS3Client? client;
        string? bucketName;

        public AmazonClient(AmazonS3Client client, string bucketName)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.bucketName = bucketName ?? throw new ArgumentNullException(nameof(bucketName));
        }

        public async Task DeleteBlobAsync(string key, CancellationToken token)
        {
            using var client = new AmazonS3Client();
            await client.DeleteObjectAsync(bucketName, key, token);
        }

        public async Task<byte[]?> FindBlobAsync(string key, CancellationToken token)
        {
            using var client = new AmazonS3Client();
            var response = await client.GetObjectAsync(bucketName, key, token);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new AmazonS3Exception($"Failed to get object from S3. HttpStatusCode: {response.HttpStatusCode}.");

            using var memoryStream = new MemoryStream();
            await response.ResponseStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public async Task<IStreamMetadata> FindBlobMetadataAsync(string key, CancellationToken token)
        {
            using var client = new AmazonS3Client();

            var response = await client.GetObjectMetadataAsync(bucketName, key, token);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                throw new AmazonS3Exception($"Failed to get object from S3. HttpStatusCode: {response.HttpStatusCode}.");
            return new AmazonStreamMetadata(response.Metadata);
        }

        public async Task UploadBlobAsync(string key, byte[] data, IStreamMetadata metadata, CancellationToken token)
        {
            using var client = new AmazonS3Client();
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = new MemoryStream(data),
                AutoCloseStream = true,
                AutoResetStreamPosition = true
            };
            var objectMetadata = new AmazonStreamMetadata(metadata);
            objectMetadata.Keys
                .ToList()
                .ForEach(k => request.Metadata.Add(k, objectMetadata[k]));

            var response = await client.PutObjectAsync(request, token);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new AmazonS3Exception($"Failed to put object to S3. HttpStatusCode: {response.HttpStatusCode}.");
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                client!.Dispose();
                client = null;
                bucketName = null;
                disposedValue = true;

            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
