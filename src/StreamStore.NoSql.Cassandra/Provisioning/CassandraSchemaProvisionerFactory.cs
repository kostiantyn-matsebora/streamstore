﻿using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Provisioning;

namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ICassandraStorageConfigurationProvider configProvider;
        readonly CassandraTenantClusterRegistry clusterRegistry;

        public CassandraSchemaProvisionerFactory(
            ICassandraStorageConfigurationProvider configProvider, 
            CassandraTenantClusterRegistry clusterRegistry)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            var configuration = configProvider.GetStorageConfiguration(tenantId);
            var sessionFactory = new CassandraSessionFactory(clusterRegistry.GetCluster(tenantId), configuration);
            var contextFactory = new CassandraStreamRepositoryFactory(sessionFactory, configuration);
            return new CassandraSchemaProvisioner(contextFactory);
        }
    }
}