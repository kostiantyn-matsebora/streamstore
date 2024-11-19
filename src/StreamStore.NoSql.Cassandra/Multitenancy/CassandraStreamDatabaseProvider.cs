using StreamStore.Multitenancy;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraStreamDatabaseProvider : ITenantStreamDatabaseProvider
    {
        readonly CassandraTenantClusterRegistry clusterRegistry;
        readonly ICassandraStorageConfigurationProvider configProvider;
        readonly TypeMapFactory typeMapFactory;

        public CassandraStreamDatabaseProvider(
            CassandraTenantClusterRegistry clusterRegistry,
            ICassandraStorageConfigurationProvider configProvider,
            TypeMapFactory typeMapFactory)
        {
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider)); 
            this.typeMapFactory = typeMapFactory.ThrowIfNull(nameof(typeMapFactory));
        }

        public IStreamDatabase GetDatabase(Id tenantId)
        {
            var config = configProvider.GetStorageConfiguration(tenantId);
            var sessionFactory = new CassandraSessionFactory(clusterRegistry.GetCluster(tenantId),config);
            var contextFactory = new DataContextFactory(typeMapFactory, sessionFactory, config);
            return new CassandraStreamDatabase(contextFactory);
        }
    }
}
