using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;

namespace StreamStore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureStreamStore(this IServiceCollection services, Action<IStreamStoreConfigurator>? configure = default)
        {
            var configurator = new StreamStoreConfigurator();
            configure?.Invoke(configurator);
            return configurator.Configure(services);
        }

        public static IServiceCollection NewConfigureStreamStore(this IServiceCollection services, Action<INewStreamStoreConfigurator>? configure = default)
        {
            var configurator = new NewStreamStoreConfigurator();
            configure?.Invoke(configurator);
            return configurator.Configure(services);
        }
    }
}
