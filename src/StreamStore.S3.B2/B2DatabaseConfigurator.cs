using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;


namespace StreamStore.S3.B2
{
    public sealed class B2DatabaseConfigurator: B2StreamDatabaseSettingsBuilder
    {
        readonly IServiceCollection services;

        B2StreamDatabaseSettingsBuilder builder = new B2StreamDatabaseSettingsBuilder();

        public B2DatabaseConfigurator(IServiceCollection services)
        {
            this.services = services;
            services.AddSingleton<IS3Factory, B2S3Factory>();
            services.AddSingleton<IStreamDatabase, S3StreamDatabase>();
        }

        public override IServiceCollection Configure()
        {
            var settings = builder.Build();
            services.AddSingleton(settings);
            return services;
        }

        public IServiceCollection ReadFromConfig(ConfigurationManager configuration)
        {
            var section = configuration.GetSection("streamStore:b2");
            if (!section.Exists())
                throw new InvalidOperationException("streamStore:b2 configuration section not found.");

            builder.WithCredentials(
            section.GetSection("applicationKeyId").Value!,
            section.GetSection("applicationKey").Value!)
                 .WithBucketId(section.GetSection("bucketId").Value!)
                 .WithBucketName(section.GetSection("bucketName").Value!);

            var settings = builder.Build();
            services.AddSingleton(settings);

            return services;
        }
    }
}
