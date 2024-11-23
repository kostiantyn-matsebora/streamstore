using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Provisioning;

namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ICassandraStorageConfigurationProvider configProvider;
        readonly ICassandraTenantClusterRegistry clusterRegistry;
        readonly MappingConfiguration mapping;

        public CassandraSchemaProvisionerFactory(ICassandraStorageConfigurationProvider configProvider, ICassandraTenantClusterRegistry clusterRegistry, MappingConfiguration mapping)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
            this.mapping = mapping.ThrowIfNull(nameof(mapping));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            var configuration = configProvider.GetStorageConfiguration(tenantId);
            var sessionFactory = new CassandraSessionFactory(clusterRegistry.GetCluster(tenantId), configuration);
            return new CassandraSchemaProvisioner(sessionFactory, mapping, configuration);
        }
    }
}
