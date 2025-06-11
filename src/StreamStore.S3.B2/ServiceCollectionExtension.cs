using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Storage;

namespace StreamStore.S3.B2
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddB2(this IServiceCollection services, Action<B2StorageConfigurator> configure)
        {
            configure.ThrowIfNull(nameof(configure));
            var configurator = new B2StorageConfigurator();
            configure(configurator);
            return services.ConfigurePersistence(new StorageConfigurator(configurator));
        }
    }
}
