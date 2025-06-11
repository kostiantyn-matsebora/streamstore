using Microsoft.Extensions.DependencyInjection;
using StreamStore.InMemory.Configuration;
using StreamStore.Storage;


namespace StreamStore.InMemory.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection UseInMemoryStorage(this IServiceCollection services)
        {
            services.ConfigurePersistence(new StorageConfigurator());
            return services;
        }

        public static IServiceCollection UseInMemoryStorageWithMultitenancy(this IServiceCollection services)
        {
            services.ConfigurePersistenceMultitenancy(new StorageConfigurator(), new MultitenancyConfigurator());
            return services;
        }
    }
}
