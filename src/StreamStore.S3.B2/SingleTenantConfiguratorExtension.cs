using System;
using Microsoft.Extensions.Configuration;

namespace StreamStore.S3.B2
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantConfigurator UseB2Storage(this ISingleTenantConfigurator configurator, Action<B2StorageConfigurator>? configure = null)
        {
            return configurator.UseStorage<S3StreamStorage>(services =>
            {
                var configurator = new B2StorageConfigurator(services);
                configure?.Invoke(configurator);
                configurator.Configure();
            });

        }

        public static ISingleTenantConfigurator UseB2Storage(this ISingleTenantConfigurator configurator, IConfiguration configuration)
        {
            return configurator.UseStorage<S3StreamStorage>(services =>
            {
                new B2StorageConfigurator(services).ReadFromConfig(configuration);
            });

        }
    }
}
