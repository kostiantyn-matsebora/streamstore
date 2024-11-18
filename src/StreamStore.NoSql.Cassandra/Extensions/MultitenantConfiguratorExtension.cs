using System;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Multitenancy;

namespace StreamStore.NoSql.Cassandra
{
    internal static class MultitenantConfiguratorExtension
    {
        public static IMultitenancyConfigurator UseCassandra(this IMultitenancyConfigurator configurator, Action<CassandraSingleTenantConfigurator> configure)
        {
            configurator
                .UseDatabaseProvider<CassandraStreamDatabaseProvider>(services =>
                {
                    var singleConfigurator = new CassandraSingleTenantConfigurator();
                    configure(singleConfigurator);
                    singleConfigurator.Configure(services);
                });

            return configurator;
        }
    }
}
