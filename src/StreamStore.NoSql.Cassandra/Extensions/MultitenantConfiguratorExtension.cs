﻿using System;
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
                .UseStorageProvider<CassandraStreamStorageProvider>(services =>
                {
                    var multitenancyConfigurator = new CassandraMultitenantConfigurator();
                    configure(multitenancyConfigurator);
                    multitenancyConfigurator.Configure(services);
                });

            return configurator;
        }
    }
}
