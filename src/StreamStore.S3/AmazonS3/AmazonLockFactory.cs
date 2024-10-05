

using System;

using Amazon.S3;

using StreamStore.S3.Lock;
using StreamStore.S3.Client;

namespace StreamStore.S3.AmazonS3
{
    internal class AmazonLockFactory : IS3StreamLockFactory
    {
        readonly S3StreamDatabaseSettings? settings;

        public AmazonLockFactory(S3StreamDatabaseSettings settings)
        {

            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public IS3StreamLock CreateLock(Id streamId, S3StreamLockPolicy policy)
        {
            return policy switch
            {
                S3RetainLockPolicy retainLockPolicy => CreateLockInternal(streamId, retainLockPolicy),
                S3HoldLockPolicy _ => CreateLockInternal(streamId),
                _ => throw new NotImplementedException($"Lock policy {policy.GetType().Name} is not supported")
            };
        }

        private IS3StreamLock CreateLockInternal(Id streamId, S3RetainLockPolicy policy)
        {
            return new AmazonRetainLock(streamId, policy.Period, settings!.BucketName!, new AmazonS3Client());
        }

        private IS3StreamLock CreateLockInternal(Id streamId)
        {
            return new AmazonHoldLock(streamId, settings!.BucketName!, new AmazonS3Client());
        }
    }
}
