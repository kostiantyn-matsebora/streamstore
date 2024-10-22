using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;

namespace StreamStore
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureStreamStore(this IServiceCollection services, Action<StreamStoreConfigurator>? configure = default)
        {
            var configurator = new StreamStoreConfigurator(services);
            configure?.Invoke(configurator);
            configurator.Configure();
            return services;
        }
    }
}
