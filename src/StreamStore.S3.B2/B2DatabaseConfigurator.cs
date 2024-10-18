using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.S3.Client;



namespace StreamStore.S3.B2
{
    public sealed class B2DatabaseConfigurator: B2StreamDatabaseSettingsBuilder
    {
        readonly IServiceCollection services;

        public B2DatabaseConfigurator(IServiceCollection services)
        {
            this.services = services;
            services.AddSingleton<IS3LockFactory, B2S3Factory>();
            services.AddSingleton<IS3ClientFactory, B2S3Factory>();
            services.AddSingleton<IStreamDatabase, S3StreamDatabase>();
            services.AddSingleton<IStorageClientFactory, BackblazeClientFactory>();
        }

        public override IServiceCollection Configure()
        {
            var settings = Build();
            services.AddSingleton(settings);
            return services;
        }

        public IServiceCollection ReadFromConfig(IConfiguration configuration)
        {
            var section = configuration.GetSection("streamStore:b2");
            if (section == null)
                throw new InvalidOperationException("streamStore:b2 configuration section not found.");

            WithCredentials(
            section!.GetSection("applicationKeyId").Value!,
            section.GetSection("applicationKey").Value!)
                 .WithBucketId(section.GetSection("bucketId").Value!)
                 .WithBucketName(section.GetSection("bucketName").Value!);

            var settings = Build();
            services.AddSingleton(settings);

            return services;
        }
    }
}
