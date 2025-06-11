using System;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore.S3.AWS
{
    public class AWSS3StorageSettings
    {

        public string? BucketName { get; internal set; }

        public string? Delimiter { get; internal set; }

        internal AWSS3StorageSettings() { }

        public TimeSpan InMemoryLockTTL { get; internal set; }
    }

    public class AWSS3StorageConfigurationBuilder : IConfigurator
    {
        string? bucketName;

        TimeSpan ttl = TimeSpan.FromSeconds(30);

        public AWSS3StorageConfigurationBuilder WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }


        public AWSS3StorageConfigurationBuilder WithInMemoryLockTTL(TimeSpan ttl)
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

