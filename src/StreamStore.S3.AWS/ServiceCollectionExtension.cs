using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Storage;

namespace StreamStore.S3.AWS
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseAmazonS3(this IServiceCollection services)
        {
            return UseAmazonS3(services, new AWSS3StorageConfigurationBuilder());
        }

        public static IServiceCollection UseAmazonS3(this IServiceCollection services, Action<AWSS3StorageConfigurationBuilder> configure)
        {
            var builder = new AWSS3StorageConfigurationBuilder();
            configure(builder);
            return UseAmazonS3(services, builder);
        }


        static IServiceCollection UseAmazonS3(IServiceCollection services, AWSS3StorageConfigurationBuilder configure)
        {
            return services.ConfigurePersistence(new StorageConfigurator(configure.Build()));
        }
    }
}
