using System;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
