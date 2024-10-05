using Amazon.S3.Model;
using Amazon.S3;
using System.Threading.Tasks;
using System.Threading;
using System;
using StreamStore.S3.Lock;
using System.Linq;


namespace StreamStore.S3.AmazonS3
{
    internal sealed class AmazonStreamLock: IS3StreamLock
    {
        readonly Id streamId;
        string? bucketName;
        AmazonS3Client? client;

        public AmazonStreamLock(Id streamId, string bucketName, AmazonS3Client client) 
        {
            if (streamId == Id.None)
                throw new ArgumentException("Stream Id must be specified", nameof(streamId));
            this.streamId = streamId;

            if (string.IsNullOrWhiteSpace(bucketName))
                throw new ArgumentException("Bucket name must be specified", nameof(bucketName));
            this.bucketName = bucketName;
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<bool> AcquireAsync(CancellationToken token)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = S3Naming.LockKey(streamId),
                ContentBody = "lock", // for debugging purposes
                //ObjectLockLegalHoldStatus = ObjectLockLegalHoldStatus.On,
                CalculateContentMD5Header = true,
            };
            var lockMetadata = new AmazonStreamLockMetadata();

            lockMetadata.Keys
                .ToList()
                .ForEach(key => request.Metadata.Add(key, lockMetadata[key]));

            var response = await client!.PutObjectAsync(request, token);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task ReleaseAsync(CancellationToken token)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = S3Naming.LockKey(streamId),
            };
            
            await client!.DeleteObjectAsync(request, token);
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
