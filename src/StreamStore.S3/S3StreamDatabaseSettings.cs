using System;
using StreamStore.S3.Client;

namespace StreamStore.S3
{
    public sealed class S3StreamDatabaseSettings
    {
        public string? BucketName { get; internal set;  }
        public S3StreamLockPolicy? LockPolicy { get; internal set; }

        internal S3StreamDatabaseSettings()
        {
        }

        public static S3StreamDatabaseSettingsBuilder New => new S3StreamDatabaseSettingsBuilder();

        public static S3StreamDatabaseSettings Default(string bucketName) =>
            S3StreamDatabaseSettingsBuilder.DefaultSettings(bucketName);


    }

    public sealed class S3StreamDatabaseSettingsBuilder
    {
        private string? bucketName;
        private S3StreamLockPolicy? lockPolicy;

        public static S3StreamDatabaseSettings DefaultSettings(string bucketName) =>
                   new S3StreamDatabaseSettingsBuilder()
                   .WithBucketName(bucketName)
                   .WithLockPolicy(S3StreamLockPolicy.Default)
                   .Build();
        public S3StreamDatabaseSettingsBuilder WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }

        public S3StreamDatabaseSettingsBuilder WithRetainLock(TimeSpan period)
        {
            return WithLockPolicy(new S3RetainLockPolicy(period));
        
        }

        public S3StreamDatabaseSettingsBuilder WithHoldLock()
        {
            return WithLockPolicy(new S3HoldLockPolicy());
        }

        private S3StreamDatabaseSettingsBuilder WithLockPolicy(S3StreamLockPolicy lockPolicy)
        {
            if (lockPolicy == null)
                throw new InvalidOperationException("Lock policy is required");
            this.lockPolicy = lockPolicy;
            return this;
        }

        public S3StreamDatabaseSettings Build()
        {
           if (bucketName == null)
            {
                throw new InvalidOperationException("Bucket name is required");
            }
           if (lockPolicy == null)
            {
                throw new InvalidOperationException("Lock policy is required");
            }
            return new S3StreamDatabaseSettings
            {
                BucketName = bucketName,
                LockPolicy = lockPolicy
            };
        }

    }


}