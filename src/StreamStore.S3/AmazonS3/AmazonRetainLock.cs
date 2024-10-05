

using System;
using System.Threading;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;

namespace StreamStore.S3.AmazonS3
{

    class AmazonRetainLock : AmazonLockBase
    {
        TimeSpan retentionPeriod;

        public AmazonRetainLock(Id streamId, TimeSpan retentionPeriod, string bucketName, AmazonS3Client client): base(streamId, bucketName, client)
        {
            if (retentionPeriod <= TimeSpan.Zero)
                throw new ArgumentException("Retention period must be greater than zero", nameof(retentionPeriod));
            this.retentionPeriod = retentionPeriod;
        }

        public override Task ReleaseAsync(CancellationToken token)
        {
           
            // Retention locks in Compliance mode cannot be released until the retention period
            return Task.CompletedTask;
        }

        protected override PutObjectRequest CreatePutRequest()
        {
            return  new PutObjectRequest
            {
                BucketName = $"{bucketName}_locks",
                Key = streamId,
                ContentBody = "lock",
                ObjectLockMode = ObjectLockMode.Compliance,
                ObjectLockRetainUntilDate = DateTime.UtcNow.Add(retentionPeriod)
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                base.Dispose(disposing);
                retentionPeriod = TimeSpan.Zero;
                disposedValue = true;
            }
        }
    }
}
