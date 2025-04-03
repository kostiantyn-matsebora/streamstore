using System;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore.S3.AWS
{
    public class AWSS3StorageSettings
    {

        public string? BucketName { get; internal set; }

        public string? Delimiter { get; internal set; }

        internal AWSS3StorageSettings() { }

        public static AWSS3StorageSettingsBuilder New => new AWSS3StorageSettingsBuilder();

        public TimeSpan InMemoryLockTTL { get; internal set; }
    }

    public class AWSS3StorageSettingsBuilder : IConfigurator
    {
        string? bucketName;

        TimeSpan ttl = TimeSpan.FromSeconds(30);

        public AWSS3StorageSettingsBuilder WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }


        public AWSS3StorageSettingsBuilder WithInMemoryLockTTL(TimeSpan ttl)
        {
            this.ttl = ttl;
            return this;
        }

        public AWSS3StorageSettings Build()
        {

            return new AWSS3StorageSettings
            {
                BucketName = bucketName ?? "streamstore",
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

