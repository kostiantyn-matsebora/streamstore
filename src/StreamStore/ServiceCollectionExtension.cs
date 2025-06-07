using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;

namespace StreamStore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddStreamStore(this IServiceCollection services, Action<INewStreamStoreConfigurator>? configure = default)
        {
            var configurator = new NewStreamStoreConfigurator();
            configure?.Invoke(configurator);
            return configurator.Configure(services);
        }
    }
}
