using System;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.NoSql.Cassandra.Provisioning;

namespace StreamStore.NoSql.Cassandra
{
    public static class MultitenantConfiguratorExtension
    {
        public static IMultitenancyConfigurator UseCassandra(this IMultitenancyConfigurator configurator, Action<CassandraMultitenantConfigurator> configure)
        {
            configurator
                .UseSchemaProvisionerFactory<CassandraSchemaProvisionerFactory>()
                .UseDatabaseProvider<CassandraStreamDatabaseProvider>(services =>
                {
                    var multitenancyConfigurator = new CassandraMultitenantConfigurator();
                    configure(multitenancyConfigurator);
                    multitenancyConfigurator.Configure(services);
                });

            return configurator;
        }

        public static IMultitenancyConfigurator UseCosmosDbCassandra(this IMultitenancyConfigurator configurator, Action<CosmosDbMultitenantConfigurator> configure)
        {
            configurator
                .UseSchemaProvisionerFactory<CassandraSchemaProvisionerFactory>()
                .UseDatabaseProvider<CassandraStreamDatabaseProvider>(services =>
                {
                    var multitenancyConfigurator = new CosmosDbMultitenantConfigurator();
                    configure(multitenancyConfigurator);
                    multitenancyConfigurator.Configure(services);
                });

            return configurator;
        }
    }
}
