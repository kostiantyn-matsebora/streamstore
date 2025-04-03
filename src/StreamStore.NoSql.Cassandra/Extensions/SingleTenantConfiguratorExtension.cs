using System;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Storage;
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
                .UseStorage<CassandraStreamStorage>(services =>
                {
                    var singleConfigurator = new CassandraSingleTenantConfigurator();
                    configure(singleConfigurator);
                    singleConfigurator.Configure(services);
                });
            return configurator;

        }
    }
}
