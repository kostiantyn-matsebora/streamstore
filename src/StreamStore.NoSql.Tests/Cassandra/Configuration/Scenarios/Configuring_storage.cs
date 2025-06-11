using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;

namespace StreamStore.NoSql.Tests.Cassandra.Configuration
{
    public class Configuring_storage : StorageConfiguratorScenario
    {
        protected override IStorageConfigurator CreateConfigurator()
        {
            var configurator = new StorageConfigurator();
            configurator.ConfigureCluster(builder => builder.AddContactPoint("localhost"));
            return configurator;
        }
    }
}
