using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;

namespace StreamStore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddStreamStore(this IServiceCollection services, Action<IStreamStoreConfigurator>? configure = default)
        {
            var configurator = new StreamStoreConfigurator();
            configure?.Invoke(configurator);
            return configurator.Configure(services);
        }
    }
}
