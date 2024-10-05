using System;


namespace StreamStore.S3
{
    public sealed class S3StreamDatabaseSettings
    {
        public string? BucketName { get; internal set;  }
        public string? LockBucketName { get; internal set; }

        internal S3StreamDatabaseSettings()
        {
        }

        public static S3StreamDatabaseSettingsBuilder New => new S3StreamDatabaseSettingsBuilder();

        public static S3StreamDatabaseSettings Default(string bucketName) =>
            S3StreamDatabaseSettingsBuilder.DefaultSettings(bucketName);


    }

    public sealed class S3StreamDatabaseSettingsBuilder
    {
        string? bucketName;
        string? lockBucketName;

        public static S3StreamDatabaseSettings DefaultSettings(string bucketName) =>
                   new S3StreamDatabaseSettingsBuilder()
                   .WithBucketName(bucketName)

                   .Build();

        public S3StreamDatabaseSettingsBuilder WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }

        public S3StreamDatabaseSettingsBuilder WithLockBucketName(string bucketName)
        {
            this.lockBucketName = bucketName;
            return this;
        }

        public S3StreamDatabaseSettings Build()
        {
           if (bucketName == null)
              throw new InvalidOperationException("Bucket name is required");

            return new S3StreamDatabaseSettings
            {
                BucketName = bucketName,
                LockBucketName = lockBucketName ?? $"{bucketName}_locks"
            };
        }
    }
}