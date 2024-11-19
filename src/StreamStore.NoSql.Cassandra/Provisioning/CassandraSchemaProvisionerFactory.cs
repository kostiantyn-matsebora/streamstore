using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Provisioning;

namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ICassandraStorageConfigurationProvider configProvider;
        readonly CassandraTenantClusterRegistry clusterRegistry;
        readonly TypeMapFactory typeMapFactory;

        public CassandraSchemaProvisionerFactory(
            ICassandraStorageConfigurationProvider configProvider, 
            CassandraTenantClusterRegistry clusterRegistry,
            TypeMapFactory typeMapFactory)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
            this.typeMapFactory = typeMapFactory.ThrowIfNull(nameof(typeMapFactory));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            var sessionFactory = new CassandraSessionFactory(clusterRegistry.GetCluster(tenantId), configProvider.GetStorageConfiguration(tenantId));
            var contextFactory = new DataContextFactory(typeMapFactory, sessionFactory, configProvider.GetStorageConfiguration(tenantId));
            return new CassandraSchemaProvisioner(contextFactory);
        }
    }
}
