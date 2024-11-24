using StreamStore.Multitenancy;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraStreamDatabaseProvider : ITenantStreamDatabaseProvider
    {
        private readonly ICassandraTenantMapperProvider mapperProvider;
        readonly ICassandraTenantStorageConfigurationProvider configProvider;


        public CassandraStreamDatabaseProvider(
            ICassandraTenantMapperProvider mapperProvider,
            ICassandraTenantStorageConfigurationProvider configProvider)
        {
            this.mapperProvider = mapperProvider.ThrowIfNull(nameof(mapperProvider));
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
        }

        public IStreamDatabase GetDatabase(Id tenantId)
        {
            return new CassandraStreamDatabase(
                        new CassandraStreamRepositoryFactory(
                            mapperProvider.GetMapperProvider(tenantId),
                            configProvider.GetConfiguration(tenantId)));
        }
    }
}
