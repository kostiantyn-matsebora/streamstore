using Microsoft.Extensions.DependencyInjection;
using StreamStore.S3.Client;

namespace StreamStore.S3.AWS
{
    public sealed class AWSS3DatabaseConfigurator: AWSS3DatabaseSettingsBuilder
    {
        readonly IServiceCollection services;

        public AWSS3DatabaseConfigurator(IServiceCollection services)
        {
            this.services = services ?? throw new System.ArgumentNullException(nameof(services));
            services.AddSingleton<IAmazonS3ClientFactory, AmazonS3ClientFactory>();
            services.AddSingleton<IS3LockFactory, AWSS3Factory>();
            services.AddSingleton<IS3ClientFactory, AWSS3Factory>();
            services.AddSingleton<IStreamDatabase, S3StreamDatabase>();
            
        }

        public override IServiceCollection Configure()
        {
            var settings = Build();
            services.AddSingleton(settings);
            return services;
        }
    }
}
