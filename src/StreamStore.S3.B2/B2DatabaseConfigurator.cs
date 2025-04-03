using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.S3.Client;
using StreamStore.S3.Storage;



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
            services.AddSingleton<IStorageClientFactory, BackblazeClientFactory>();
            services.AddSingleton<IStreamStorage, S3StreamDatabase>();
            services.AddSingleton<IStreamReader, S3StreamDatabase>();
            services.AddSingleton<IS3StorageFactory, S3StorageFactory>();
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

             WithCredential(section!.GetSection("applicationKeyId").Value!, section!.GetSection("applicationKey").Value!)
            .WithBucketId(section.GetSection("bucketId").Value!)
            .WithBucketName(section.GetSection("bucketName").Value!);

            var settings = Build();
            services.AddSingleton(settings);

            return services;
        }
    }
}
