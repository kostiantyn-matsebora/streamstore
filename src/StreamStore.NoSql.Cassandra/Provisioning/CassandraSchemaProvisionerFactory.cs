using Cassandra.Mapping;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Provisioning;

namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ICassandraTenantStorageConfigurationProvider configProvider;
        readonly ICassandraTenantClusterRegistry clusterRegistry;

        public CassandraSchemaProvisionerFactory(
            ICassandraTenantStorageConfigurationProvider configProvider, 
            ICassandraTenantClusterRegistry clusterRegistry)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            var config = configProvider.GetConfiguration(tenantId);
            return new CassandraSchemaProvisioner(
                new CassandraSessionFactory(clusterRegistry.GetCluster(tenantId), config), config);
        }
    }
}
