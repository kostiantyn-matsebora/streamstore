using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace StreamStore.ExampleBase.Configuration
{
    [ExcludeFromCodeCoverage]
    internal class StorageConfiguratorRegistry
    {
        readonly Dictionary<string, StorageConfigurator> configurators = new Dictionary<string, StorageConfigurator>();

        public StorageConfigurator GetOrAdd(string storageName)
        {
            if (configurators.TryGetValue(storageName, out var configurator))
            {
                return configurator;
            }

            configurator = new StorageConfigurator();
            configurators.Add(storageName, configurator);
            return configurator;
        }

        public StorageConfigurator Get(string storageName)
        {
            if (configurators.TryGetValue(storageName, out var configurator))
            {
                return configurator;
            }

            throw new InvalidOperationException($"Storage configurator for '{storageName}' not found.");
        }
    }
}
