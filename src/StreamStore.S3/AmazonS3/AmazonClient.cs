using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;


namespace StreamStore.S3.AmazonS3
{
    internal class AmazonClient : IS3Client
    {
        private bool disposedValue;

        public async Task DeleteBlobAsync(string bucketName, string key, CancellationToken token)
        {
            using var client = new AmazonS3Client();
            await client.DeleteObjectAsync(bucketName, key, token);
        }

        public async Task<MemoryStream?> FindBlobAsync(string bucketName, string key, CancellationToken token)
        {
            using var client = new AmazonS3Client();
            var response = await client.GetObjectAsync(bucketName, key, token);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                throw new AmazonS3Exception($"Failed to get object from S3. HttpStatusCode: {response.HttpStatusCode}.");

            using (var memoryStream = new MemoryStream())
            {
                await response.ResponseStream.CopyToAsync(memoryStream);
                return memoryStream;
            }
        }

        public async Task<IMetadataCollection> FindBlobMetadataAsync(string bucketName, string key, CancellationToken token)
        {
            using var client = new AmazonS3Client();

            var response = await client.GetObjectMetadataAsync(bucketName, key, token);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                throw new AmazonS3Exception($"Failed to get object from S3. HttpStatusCode: {response.HttpStatusCode}.");
            return new AmazonMetadataCollection(response.Metadata);
        }

        public async Task UploadBlobAsync(string bucketName, string key, MemoryStream stream, IDictionary<string, string> metadata, CancellationToken token)
        {
            using var client = new AmazonS3Client();
            var request = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                InputStream = stream,
            };
            foreach (var item in metadata)
            {
                request.Metadata.Add(item.Key, item.Value);
            }
            await client.PutObjectAsync(request, token);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
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
