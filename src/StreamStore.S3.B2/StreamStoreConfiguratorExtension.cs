using System;
using Microsoft.Extensions.Configuration;

namespace StreamStore.S3.B2
{
    public static class StreamStoreConfiguratorExtension
    {

        public static IStreamStoreConfigurator UseB2Database(this IStreamStoreConfigurator configurator, Action<B2DatabaseConfigurator>? configure = null)
        {
            return configurator.WithSingleTenant(c =>
            {
                c.UseDatabase<S3StreamDatabase>(services =>
                {
                    var configurator = new B2DatabaseConfigurator(services);
                    configure?.Invoke(configurator);
                    configurator.Configure();
                });
            });
        }

        public static IStreamStoreConfigurator UseB2Database(this IStreamStoreConfigurator configurator, IConfiguration configuration)
        {
            return configurator.WithSingleTenant(c =>
            {
                c.UseDatabase<S3StreamDatabase>(services =>
                {
                    new B2DatabaseConfigurator(services).ReadFromConfig(configuration);
                });
            });
        }
    }
}
