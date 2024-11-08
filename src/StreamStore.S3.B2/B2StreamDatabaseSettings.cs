using System;
using System.Net;
using System.Security;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore.S3.B2
{
    public sealed class B2StreamDatabaseSettings
    {
        public string? BucketId { get; internal set; }

        public string? BucketName { get; internal set; }

        public NetworkCredential? Credential { get; internal set; }

        public string? Delimiter { get; internal set; }

        internal B2StreamDatabaseSettings() { }

        public static B2StreamDatabaseSettingsBuilder New => new B2StreamDatabaseSettingsBuilder();

        public TimeSpan InMemoryLockTTL { get; internal set; }
    }

    public class B2StreamDatabaseSettingsBuilder: IConfigurator
    {
        string? bucketId;
        string? bucketName;


        NetworkCredential? credentials;
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

        public B2StreamDatabaseSettingsBuilder WithCredential(string accessKeyId, string accessKey)
        {
            credentials = new NetworkCredential(accessKeyId, accessKey);
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

