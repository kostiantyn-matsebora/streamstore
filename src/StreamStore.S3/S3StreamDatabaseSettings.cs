using System;


namespace StreamStore.S3
{
    public sealed class S3StreamDatabaseSettings
    {
        public string? BucketName { get; internal set;  }

        public S3Credentials? Credentials { get; internal set; }

        public string? Endpoint { get; internal set; }

        internal S3StreamDatabaseSettings()
        {
        }

        public static S3StreamDatabaseSettingsBuilder New => new S3StreamDatabaseSettingsBuilder();

        public static S3StreamDatabaseSettings Default(string bucketName) =>
            S3StreamDatabaseSettingsBuilder.DefaultSettings(bucketName);


    }

    public class S3Credentials
    {
        public string AccessKeyId { get; }
        public string AccessKey { get;  }

        internal S3Credentials(string accessKey, string accessKeyId)
        {
            AccessKeyId = accessKeyId;
            AccessKey = accessKey;
        }
    }

    public sealed class S3StreamDatabaseSettingsBuilder
    {
        string? bucketName;
        S3Credentials? credentials;
        string? endpoint;

        public static S3StreamDatabaseSettings DefaultSettings(string bucketName) =>
                   new S3StreamDatabaseSettingsBuilder()
                   .WithBucketName(bucketName)
                   .Build();

        public S3StreamDatabaseSettingsBuilder WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }


        public S3StreamDatabaseSettingsBuilder WithCredentials(string keyId, string key)
        {
            credentials = new S3Credentials(keyId, key);
            return this;
        }

        public S3StreamDatabaseSettingsBuilder WithEndpoint(string endpoint)
        {
            this.endpoint = endpoint;
            return this;
        }

        public S3StreamDatabaseSettings Build()
        {
            if (credentials == null)
                throw new InvalidOperationException("Credentials are required.");
            if (endpoint == null)
                throw new InvalidOperationException("Endpoint is required.");
            return new S3StreamDatabaseSettings
            {
                BucketName = bucketName ?? "streamstore",
                Credentials = credentials,
                Endpoint = endpoint
            };
        }
    }
}
