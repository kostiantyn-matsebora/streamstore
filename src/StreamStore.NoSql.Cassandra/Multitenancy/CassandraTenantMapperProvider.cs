using System;
using System.Collections.Generic;
using System.Text;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal interface ICassandraTenantMapperProvider
    {
        public ICassandraMapperProvider GetMapperProvider(Id tenantId);
    }

    internal class CassandraTenantMapperProvider : ICassandraTenantMapperProvider
    {
        private readonly ICassandraTenantStorageConfigurationProvider configurationProvider;
        private readonly ICassandraTenantClusterRegistry clusterRegistry;
        private readonly ICassandraTenantMappingRegistry mappingRegistry;

        public CassandraTenantMapperProvider(
            ICassandraTenantStorageConfigurationProvider configurationProvider,
            ICassandraTenantClusterRegistry clusterRegistry,
            ICassandraTenantMappingRegistry mappingRegistry)
        {
            this.configurationProvider = configurationProvider.ThrowIfNull(nameof(configurationProvider));
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
            this.mappingRegistry = mappingRegistry.ThrowIfNull(nameof(mappingRegistry));
        }

        public ICassandraMapperProvider GetMapperProvider(Id tenantId)
        {
            var cluster = clusterRegistry.GetCluster(tenantId);
            var mapping = mappingRegistry.GetMapping(tenantId);
            var config = configurationProvider.GetConfiguration(tenantId);
            return new CassandraMapperProvider(new CassandraSessionFactory(cluster, config), mapping);
        }
    }
}
