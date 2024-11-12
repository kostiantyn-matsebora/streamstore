
using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.InMemory
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UseInMemoryDatabase(this IStreamStoreConfigurator configurator)
        {
            configurator.WithDatabase(registrator => registrator.ConfigureWith(RegisterDatabase));

            return configurator;
        }

        static void RegisterDatabase(IServiceCollection services)
        {
            services.AddSingleton<IStreamDatabase, InMemoryStreamDatabase>();
            services.AddSingleton<IStreamReader, InMemoryStreamDatabase>();
        }
    }
}
