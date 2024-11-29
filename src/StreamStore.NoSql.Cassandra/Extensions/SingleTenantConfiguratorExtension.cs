using System;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Provisioning;
using Microsoft.Extensions.Configuration;

namespace StreamStore.NoSql.Cassandra
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantConfigurator UseCassandra(this ISingleTenantConfigurator configurator, Action<CassandraSingleTenantConfigurator> configure)
        {
            configurator
                .UseSchemaProvisioner<CassandraSchemaProvisioner>()
                .UseDatabase<CassandraStreamDatabase>(services =>
                {
                    var singleConfigurator = new CassandraSingleTenantConfigurator();
                    configure(singleConfigurator);
                    singleConfigurator.Configure(services);
                });
            return configurator;

        }
    }
}
