using System;


namespace StreamStore.S3.B2
{
    public sealed class B2StreamDatabaseSettings
    {
        public string? BucketId { get; internal set; }

        public string? BucketName { get; internal set; }

        public B2S3Credentials? Credentials { get; internal set; }

        public string? Delimiter { get; internal set; }

        internal B2StreamDatabaseSettings() { }

        public static B2StreamDatabaseSettingsBuilder New => new B2StreamDatabaseSettingsBuilder();

        public TimeSpan InMemoryLockTTL { get; internal set; }
    }

    public sealed class B2StreamDatabaseSettingsBuilder
    {
        string? bucketId;
        string? bucketName;
        string delimiter = S3Naming.Delimiter;

        B2S3Credentials? credentials;
        TimeSpan ttl = TimeSpan.FromSeconds(30);

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
            credentials = new B2S3Credentials(keyId, key);
            return this;
        }

        public B2StreamDatabaseSettingsBuilder WithDelimiter(string delimiter)
        {
            this.delimiter = delimiter;
            return this;
        }

        public B2StreamDatabaseSettingsBuilder WithInMemoryLockTTL(TimeSpan ttl)
        {
            this.ttl = ttl;
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
                Delimiter = delimiter,
                InMemoryLockTTL = ttl
            };
        }
    }
}
