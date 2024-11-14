using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;


namespace StreamStore.ExampleBase.Configuration
{
    [ExcludeFromCodeCoverage]
    internal class DatabaseConfiguratorRegistry
    {
        readonly Dictionary<string, DatabaseConfigurator> configurators = new Dictionary<string, DatabaseConfigurator>();

        public DatabaseConfigurator GetOrAdd(string databaseName)
        {
            if (configurators.TryGetValue(databaseName, out var configurator))
            {
                return configurator;
            }

            configurator = new DatabaseConfigurator();
            configurators.Add(databaseName, configurator);
            return configurator;
        }

        public DatabaseConfigurator Get(string databaseName)
        {
            if (configurators.TryGetValue(databaseName, out var configurator))
            {
                return configurator;
            }

            throw new InvalidOperationException($"Database configurator for '{databaseName}' not found.");
        }
    }
}
