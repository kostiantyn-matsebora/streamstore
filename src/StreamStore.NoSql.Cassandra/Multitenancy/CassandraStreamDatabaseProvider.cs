using StreamStore.Multitenancy;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraStreamDatabaseProvider : ITenantStreamDatabaseProvider
    {
        private readonly ICassandraTenantMapperProvider mapperProvider;
        readonly ICassandraTenantStorageConfigurationProvider configProvider;
        private readonly ICassandraCqlQueriesProvider cqlQueriesProvider;

        public CassandraStreamDatabaseProvider(
            ICassandraTenantMapperProvider mapperProvider,
            ICassandraTenantStorageConfigurationProvider configProvider,
            ICassandraCqlQueriesProvider cqlQueriesProvider)
        {
            this.mapperProvider = mapperProvider.ThrowIfNull(nameof(mapperProvider));
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.cqlQueriesProvider = cqlQueriesProvider.ThrowIfNull(nameof(cqlQueriesProvider));
        }

        public IStreamStorage GetDatabase(Id tenantId)
        {
            var config = configProvider.GetConfiguration(tenantId);
            return new CassandraStreamDatabase(mapperProvider.GetMapperProvider(tenantId), cqlQueriesProvider.GetCqlQueries(config), config);
        }
    }
}
