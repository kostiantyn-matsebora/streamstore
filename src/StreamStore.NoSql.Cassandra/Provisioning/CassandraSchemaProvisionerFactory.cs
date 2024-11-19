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
        readonly DataContextFactory contextFactory;

        public CassandraSchemaProvisionerFactory(ICassandraStorageConfigurationProvider configProvider, CassandraTenantClusterRegistry clusterRegistry, DataContextFactory contextFactory)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
            this.contextFactory = contextFactory.ThrowIfNull(nameof(contextFactory)); 
        }
        public ISchemaProvisioner Create(Id tenantId)
        {
            var sessionFactory = new CassandraSessionFactory(clusterRegistry.GetCluster(tenantId), configProvider.GetStorageConfiguration(tenantId));
            return new CassandraSchemaProvisioner(sessionFactory, contextFactory);
        }
    }
}
