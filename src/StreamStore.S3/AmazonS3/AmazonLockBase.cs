

using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using StreamStore.S3.Lock;

namespace StreamStore.S3.AmazonS3
{
    abstract class AmazonLockBase: IS3StreamLock
    {
        protected Id streamId;
        protected string? bucketName;
        protected AmazonS3Client? client;
        protected bool disposedValue;

        public AmazonLockBase(Id streamId,string bucketName, AmazonS3Client client)
        {
            if (streamId == Id.None)
                throw new ArgumentException("Stream Id must be specified", nameof(streamId));
            this.streamId = streamId;

            if (string.IsNullOrWhiteSpace(bucketName))
                throw new ArgumentException("Bucket name must be specified", nameof(bucketName));
            this.bucketName = bucketName;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        protected abstract PutObjectRequest CreatePutRequest();

        public async Task<bool> AcquireAsync(CancellationToken token)
        {

            using var client = new AmazonS3Client();

            var response = await client.PutObjectAsync(CreatePutRequest(), token);

            return response.HttpStatusCode != System.Net.HttpStatusCode.OK;
        }

        public abstract Task ReleaseAsync(CancellationToken token);

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                client!.Dispose();
                client = null;
                bucketName = null;
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
