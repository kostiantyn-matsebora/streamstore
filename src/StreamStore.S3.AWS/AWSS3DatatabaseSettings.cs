using System;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore.S3.AWS
{
    public class AWSS3DatabaseSettings
    {

        public string? BucketName { get; internal set; }

        public string? Delimiter { get; internal set; }

        internal AWSS3DatabaseSettings() { }

        public static AWSS3DatabaseSettingsBuilder New => new AWSS3DatabaseSettingsBuilder();

        public TimeSpan InMemoryLockTTL { get; internal set; }
    }

    public class AWSS3DatabaseSettingsBuilder : IConfigurator
    {
        string? bucketName;

        TimeSpan ttl = TimeSpan.FromSeconds(30);

        public AWSS3DatabaseSettingsBuilder WithBucketName(string bucketName)
        {
            this.bucketName = bucketName;
            return this;
        }


        public AWSS3DatabaseSettingsBuilder WithInMemoryLockTTL(TimeSpan ttl)
        {
            this.ttl = ttl;
            return this;
        }

        public AWSS3DatabaseSettings Build()
        {

            return new AWSS3DatabaseSettings
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

