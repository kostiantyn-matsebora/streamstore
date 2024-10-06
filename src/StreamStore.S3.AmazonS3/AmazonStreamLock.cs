using Amazon.S3.Model;
using Amazon.S3;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using StreamStore.S3.Client;

namespace StreamStore.S3.AmazonS3
{
    internal sealed class AmazonStreamLock : IS3StreamLock
    {
        readonly Id streamId;
        string? bucketName;
        AmazonS3Client? client;
        string? versionId;

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
                ObjectLockMode = ObjectLockMode.Governance,
                ObjectLockRetainUntilDate = DateTime.UtcNow.AddMinutes(1),
                CalculateContentMD5Header = true,
            };
            var lockMetadata = new S3StreamLockMetadata();

            lockMetadata.Keys
                .ToList()
                .ForEach(key => request.Metadata.Add(key, lockMetadata[key]));

            var response = await client!.PutObjectAsync(request, token);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                return false;
            versionId = response.VersionId;
            return true;
        }

        public async Task ReleaseAsync(CancellationToken token)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = S3Naming.LockKey(streamId),
                VersionId = null
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
                versionId = null;
            }
        }
    }
}
