using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Storage.Configuration;

namespace StreamStore.Storage
{
	public static class ServiceCollectionExtension
	{
		public static IServiceCollection ConfigurePersistenceMultitenancy(this IServiceCollection services, StorageConfiguratorBase configurator, MultitenancyConfiguratorBase multitenancyConfigurator)
		{
            configurator.ThrowIfNull(nameof(configurator));
            multitenancyConfigurator.ThrowIfNull(nameof(multitenancyConfigurator));

            var storageServices = 
                new StorageDependencyBuilder()
                    .WithStorageConfigurator(configurator)
                    .WithMultitenancyConfigurator(multitenancyConfigurator)
                    .Build();
            services.CopyFrom(storageServices);
            return services;
        }
        public static IServiceCollection ConfigurePersistence(this IServiceCollection services, StorageConfiguratorBase configurator)
        {
            configurator.ThrowIfNull(nameof(configurator));

            var storageServices =
               new StorageDependencyBuilder()
                   .WithStorageConfigurator(configurator)
                   .Build();
            services.CopyFrom(storageServices);
            return services;
        }

    }
}
