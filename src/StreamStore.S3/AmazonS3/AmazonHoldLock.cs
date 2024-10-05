
using Amazon.S3.Model;
using Amazon.S3;
using System.Threading.Tasks;
using System.Threading;


namespace StreamStore.S3.AmazonS3
{
    internal class AmazonHoldLock: AmazonLockBase
    {
        public AmazonHoldLock(Id streamId, string bucketName, AmazonS3Client client) : base(streamId, bucketName, client)
        {
        }

        public override async Task ReleaseAsync(CancellationToken token)
        {
            var request = new PutObjectLegalHoldRequest
            {
                BucketName = $"{bucketName}_locks",
                Key = streamId,
                LegalHold = new ObjectLockLegalHold
                {
                    Status = ObjectLockLegalHoldStatus.Off
                },


            };
            await client!.PutObjectLegalHoldAsync(request, token);
        }

        protected override PutObjectRequest CreatePutRequest()
        {
            return new PutObjectRequest
            {
                BucketName = $"{bucketName}_locks",
                Key = streamId,
                ContentBody = "lock",
                ObjectLockLegalHoldStatus = ObjectLockLegalHoldStatus.On
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                base.Dispose(disposing);
                disposedValue = true;
            }
        }
    }
}
