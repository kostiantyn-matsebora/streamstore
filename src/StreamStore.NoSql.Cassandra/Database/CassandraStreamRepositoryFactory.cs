using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamRepositoryFactory : ICassandraStreamRepositoryFactory
    {
        readonly CassandraStorageConfiguration config;
        readonly ICassandraMapperProvider mapperProvider;

        public CassandraStreamRepositoryFactory(ICassandraMapperProvider mapperProvider, CassandraStorageConfiguration config)
        {
            this.config = config.ThrowIfNull(nameof(config));
            this.mapperProvider = mapperProvider.ThrowIfNull(nameof(mapperProvider));
        }

        public ICassandraStreamRepository Create()
        {
            return new CassandraStreamRepository(mapperProvider.OpenMapper(), config);
        }
    }
}
