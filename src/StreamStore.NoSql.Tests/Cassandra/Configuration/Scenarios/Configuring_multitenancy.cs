using Microsoft.Extensions.DependencyInjection;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;

namespace StreamStore.NoSql.Tests.Cassandra.Configuration
{
    public class Configuring_multitenancy: MultitenancyConfiguratorScenario
    {
        protected override IMultitenancyConfigurator CreateConfigurator()
        {
            return new MultitenancyConfigurator(CassandraMode.Cassandra);
        }

        protected override void ConfigureRequiredDependencies(IServiceCollection services)
        {
            new StorageConfigurator().Configure(services);

        }
    }
}
