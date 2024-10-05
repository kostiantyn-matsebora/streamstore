using System;
using Amazon.S3;
using StreamStore.S3.Lock;


namespace StreamStore.S3.AmazonS3
{
    internal sealed class AmazonLockFactory : IS3StreamLockFactory
    {
        readonly S3StreamDatabaseSettings? settings;

        public AmazonLockFactory(S3StreamDatabaseSettings settings)
        {

            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IS3StreamLock CreateLock(Id streamId)
        {
            return new AmazonStreamLock(streamId, settings!.LockBucketName!, new AmazonS3Client());
        }
    }
}
