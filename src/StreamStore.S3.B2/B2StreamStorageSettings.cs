using System;
using System.Net;
using System.Security;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore.S3.B2
{
    public sealed class B2StreamStorageSettings
    {
        public string? BucketId { get; internal set; }

        public string? BucketName { get; internal set; }

        public NetworkCredential? Credential { get; internal set; }

        public string? Delimiter { get; internal set; }

        internal B2StreamStorageSettings() { }

        public static B2StreamStorageSettingsBuilder New => new B2StreamStorageSettingsBuilder();

        public TimeSpan InMemoryLockTTL { get; internal set; }
    }

    public class B2StreamStorageSettingsBuilder: IConfigurator
    {
        string? bucketId;
        string? bucketName;


        NetworkCredential? credentials;
        TimeSpan ttl = TimeSpan.FromSeconds(30);

        public B2StreamStorageSettingsBuilder WithBucketId(string bucketId)
        {
            this.bucketId = bucketId;
            return this;
        }

        public B2StreamStorageSettingsBuilder WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }

        public B2StreamStorageSettingsBuilder WithCredential(string accessKeyId, string accessKey)
        {
            credentials = new NetworkCredential(accessKeyId, accessKey);
            return this;
        }

        public B2StreamStorageSettingsBuilder WithInMemoryLockTTL(TimeSpan ttl)
        {
            this.ttl = ttl;
            return this;
        }

        public B2StreamStorageSettings Build()
        {
            if (credentials == null)
                throw new InvalidOperationException("credentials are required.");
            if (bucketId == null)
                throw new InvalidOperationException("bucketId is required.");

            return new B2StreamStorageSettings
            {
                BucketId = bucketId,
                BucketName = bucketName ?? "streamstore",
                Credential = credentials,
                InMemoryLockTTL = ttl,
                Delimiter = "/"
            };
        }

        public virtual IServiceCollection Configure()
        {
            return new ServiceCollection();
        }
    }

    public interface IConfigurator
    {
        IServiceCollection Configure();
    }
}

