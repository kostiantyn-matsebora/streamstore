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

            var serviceProvider = services.BuildServiceProvider();
            var mode = serviceProvider.GetRequiredService<StreamStorageMode>();

            var storageServices = 
                new StorageDependencyBuilder()
                    .WithStorageConfigurator(configurator)
                    .WithMultitenancyConfigurator(multitenancyConfigurator)
                    .WithMode(StreamStorageMode.Multitenant)
                    .Build();
            services.CopyFrom(storageServices);
            return services;
        }
        public static IServiceCollection ConfigurePersistence(this IServiceCollection services, StorageConfiguratorBase configurator)
        {
            var storageServices =
               new StorageDependencyBuilder()
                   .WithStorageConfigurator(configurator)
                   .WithMode(StreamStorageMode.Single)
                   .Build();
            services.CopyFrom(storageServices);
            return services;
        }

    }
}
