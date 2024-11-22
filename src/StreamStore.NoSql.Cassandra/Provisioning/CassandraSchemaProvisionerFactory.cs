using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Provisioning;

namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ICassandraStorageConfigurationProvider configProvider;
        readonly ICassandraPredicateProvider predicateProvider;
        readonly ICassandraTenantClusterRegistry clusterRegistry;

        public CassandraSchemaProvisionerFactory(ICassandraStorageConfigurationProvider configProvider, ICassandraPredicateProvider predicateProvider, ICassandraTenantClusterRegistry clusterRegistry)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.predicateProvider = predicateProvider.ThrowIfNull(nameof(predicateProvider)); 
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            var configuration = configProvider.GetStorageConfiguration(tenantId);
            var sessionFactory = new CassandraSessionFactory(clusterRegistry.GetCluster(tenantId), configuration);
            var repoFactory = new CassandraStreamRepositoryFactory(sessionFactory, predicateProvider, configuration);
            return new CassandraSchemaProvisioner(repoFactory);
        }
    }
}
