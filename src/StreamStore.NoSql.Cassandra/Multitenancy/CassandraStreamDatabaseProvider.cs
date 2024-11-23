using Cassandra.Mapping;
using StreamStore.Multitenancy;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraStreamDatabaseProvider : ITenantStreamDatabaseProvider
    {
        readonly ICassandraTenantClusterRegistry clusterRegistry;
        readonly ICassandraStorageConfigurationProvider configProvider;
        readonly MappingConfiguration mapping;

        public CassandraStreamDatabaseProvider(
            ICassandraTenantClusterRegistry clusterRegistry,
            ICassandraStorageConfigurationProvider configProvider,
            MappingConfiguration mapping)
        {
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.mapping = mapping.ThrowIfNull(nameof(mapping));
        }

        public IStreamDatabase GetDatabase(Id tenantId)
        {
            var config = configProvider.GetStorageConfiguration(tenantId);
            var sessionFactory = new CassandraSessionFactory(clusterRegistry.GetCluster(tenantId),config);
            var contextFactory = new CassandraStreamRepositoryFactory(sessionFactory, config, mapping);
            return new CassandraStreamDatabase(contextFactory);
        }
    }
}
