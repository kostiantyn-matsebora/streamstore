using System;


namespace StreamStore.S3
{
    public sealed class B2StreamDatabaseSettings
    {
        public string? BucketId { get; internal set;  }

        public string? BucketName { get; internal set; }

        public S3Credentials? Credentials { get; internal set; }

        internal B2StreamDatabaseSettings()
        {
        }

        public static B2StreamDatabaseSettingsBuilder New => new B2StreamDatabaseSettingsBuilder();

        public static B2StreamDatabaseSettings Default(string bucketId) =>
            B2StreamDatabaseSettingsBuilder.DefaultSettings(bucketId);


    }

    public sealed class B2StreamDatabaseSettingsBuilder
    {
        string? bucketId;
        string? bucketName;

        S3Credentials? credentials;
  
        public static B2StreamDatabaseSettings DefaultSettings(string bucketName) =>
                   new B2StreamDatabaseSettingsBuilder()
                   .WithBucketId(bucketName)
                   .Build();

        public B2StreamDatabaseSettingsBuilder WithBucketId(string bucketId)
        {
            this.bucketId = bucketId;
            return this;
        }

        public B2StreamDatabaseSettingsBuilder WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }

        public B2StreamDatabaseSettingsBuilder WithCredentials(string keyId, string key)
        {
            credentials = new S3Credentials(keyId, key);
            return this;
        }

        public B2StreamDatabaseSettings Build()
        {
            if (credentials == null)
                throw new InvalidOperationException("credentials are required.");
            if (bucketId == null)
                throw new InvalidOperationException("bucketId is required.");
            return new B2StreamDatabaseSettings
            {
                BucketId = bucketId,
                BucketName = bucketName ?? "streamstore",
                Credentials = credentials,
            };
        }
    }
}
