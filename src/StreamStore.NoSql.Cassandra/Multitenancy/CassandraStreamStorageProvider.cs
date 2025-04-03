using StreamStore.Multitenancy;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Storage;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraStreamStorageProvider : ITenantStreamStorageProvider
    {
        private readonly ICassandraTenantMapperProvider mapperProvider;
        readonly ICassandraTenantStorageConfigurationProvider configProvider;
        private readonly ICassandraCqlQueriesProvider cqlQueriesProvider;

        public CassandraStreamStorageProvider(
            ICassandraTenantMapperProvider mapperProvider,
            ICassandraTenantStorageConfigurationProvider configProvider,
            ICassandraCqlQueriesProvider cqlQueriesProvider)
        {
            this.mapperProvider = mapperProvider.ThrowIfNull(nameof(mapperProvider));
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.cqlQueriesProvider = cqlQueriesProvider.ThrowIfNull(nameof(cqlQueriesProvider));
        }

        public IStreamStorage GetStorage(Id tenantId)
        {
            var config = configProvider.GetConfiguration(tenantId);
            return new CassandraStreamStorage(mapperProvider.GetMapperProvider(tenantId), cqlQueriesProvider.GetCqlQueries(config), config);
        }
    }
}
