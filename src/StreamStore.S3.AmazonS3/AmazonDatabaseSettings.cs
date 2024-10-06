using System;


namespace StreamStore.S3.AmazonS3
{
    public sealed class AmazonDatabaseSettings
    {
        public string? BucketName { get; internal set;  }

        public S3Credentials? Credentials { get; internal set; }

        public string? Endpoint { get; internal set; }

        internal AmazonDatabaseSettings()
        {
        }

        public static AmazonS3StreamDatabaseSettingsBuilder New => new AmazonS3StreamDatabaseSettingsBuilder();

        public static AmazonDatabaseSettings Default(string bucketName) =>
            AmazonS3StreamDatabaseSettingsBuilder.DefaultSettings(bucketName);


    }


    public sealed class AmazonS3StreamDatabaseSettingsBuilder
    {
        string? bucketName;
        S3Credentials? credentials;
        string? endpoint;

        public static AmazonDatabaseSettings DefaultSettings(string bucketName) =>
                   new AmazonS3StreamDatabaseSettingsBuilder()
                   .WithBucketName(bucketName)
                   .Build();

        public AmazonS3StreamDatabaseSettingsBuilder WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }


        public AmazonS3StreamDatabaseSettingsBuilder WithCredentials(string keyId, string key)
        {
            credentials = new S3Credentials(keyId, key);
            return this;
        }

        public AmazonS3StreamDatabaseSettingsBuilder WithEndpoint(string endpoint)
        {
            this.endpoint = endpoint;
            return this;
        }

        public AmazonDatabaseSettings Build()
        {
            if (credentials == null)
                throw new InvalidOperationException("Credentials are required.");
            if (endpoint == null)
                throw new InvalidOperationException("Endpoint is required.");
            return new AmazonDatabaseSettings
            {
                BucketName = bucketName ?? "streamstore",
                Credentials = credentials,
                Endpoint = endpoint
            };
        }
    }
}
