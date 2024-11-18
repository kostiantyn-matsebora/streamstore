using StreamStore.Multitenancy;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraStreamDatabaseProvider : ITenantStreamDatabaseProvider
    {
        readonly CassandraTenantClusterRegistry clusterRegistry;
        readonly ICassandraKeyspaceConfigurationProvider configProvider;
        readonly DataContextFactory contextFactory;

        public CassandraStreamDatabaseProvider(
            CassandraTenantClusterRegistry clusterRegistry,
            ICassandraKeyspaceConfigurationProvider configProvider,
            DataContextFactory contextFactory)
        {
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider)); 
            this.contextFactory = contextFactory.ThrowIfNull(nameof(contextFactory));
        }

        public IStreamDatabase GetDatabase(Id tenantId)
        {
            var sessionFactory = new CassandraSessionFactory(
                    clusterRegistry.GetCluster(tenantId),
                    configProvider.GetKeyspaceConfiguration(tenantId));
            return new CassandraStreamDatabase(sessionFactory, contextFactory);

        }
    }
}
