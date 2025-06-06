using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;

namespace StreamStore.S3.AWS
{
    public sealed class AWSS3StorageConfigurator: AWSS3StorageConfigurationBuilder
    {
        readonly IServiceCollection services;

        public AWSS3StorageConfigurator(IServiceCollection services, AWSS3StorageSettings settings)
        {
            this.services = services.ThrowIfNull(nameof(services));
            settings.ThrowIfNull(nameof(settings));

            services.AddSingleton(settings);
            services.AddSingleton<IAmazonS3ClientFactory, AmazonS3ClientFactory>();
            services.AddSingleton<IS3LockFactory, AWSS3Factory>();
            services.AddSingleton<IS3ClientFactory, AWSS3Factory>();
            services.AddSingleton<IStreamStorage, S3StreamStorage>();
            services.AddSingleton<IStreamReader, S3StreamStorage>();
            services.AddSingleton<IS3StorageFactory, S3StorageFactory>();
        }

        public override IServiceCollection Configure()
        {

            return services;
        }
    }
}
