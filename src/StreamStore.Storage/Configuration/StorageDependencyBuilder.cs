using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;

namespace StreamStore.Storage.Configuration
{
    internal class StorageDependencyBuilder
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        StorageConfiguratorBase storageConfigurator;
        MultitenancyConfiguratorBase? multitenancyConfigurator;

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public StorageDependencyBuilder WithStorageConfigurator(StorageConfiguratorBase configurator)
        {
            configurator.ThrowIfNull(nameof(configurator));
            storageConfigurator = configurator;
            return this;
        }

        public StorageDependencyBuilder WithMultitenancyConfigurator(MultitenancyConfiguratorBase? configurator)
        {
            multitenancyConfigurator = configurator;
            return this;
        }

        public IServiceCollection Build()
        {
            var services = new ServiceCollection();
            if (storageConfigurator == null)
            {
                throw new InvalidOperationException("Storage configurator must be set before building dependencies.");
            }
            storageConfigurator.Configure(services);

            if (multitenancyConfigurator != null)
            {
                multitenancyConfigurator.Configure(services);
            }

            return services;
        }
    }
}
