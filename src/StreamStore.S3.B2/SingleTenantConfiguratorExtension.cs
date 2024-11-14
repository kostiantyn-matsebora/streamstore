using System;
using Microsoft.Extensions.Configuration;

namespace StreamStore.S3.B2
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantDatabaseConfigurator UseB2Database(this ISingleTenantDatabaseConfigurator configurator, Action<B2DatabaseConfigurator>? configure = null)
        {
            return configurator.UseDatabase<S3StreamDatabase>(services =>
            {
                var configurator = new B2DatabaseConfigurator(services);
                configure?.Invoke(configurator);
                configurator.Configure();
            });

        }

        public static ISingleTenantDatabaseConfigurator UseB2Database(this ISingleTenantDatabaseConfigurator configurator, IConfiguration configuration)
        {
            return configurator.UseDatabase<S3StreamDatabase>(services =>
            {
                new B2DatabaseConfigurator(services).ReadFromConfig(configuration);
            });

        }
    }
}
