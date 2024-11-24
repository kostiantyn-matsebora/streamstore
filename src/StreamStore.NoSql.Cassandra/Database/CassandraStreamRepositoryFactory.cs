using Cassandra;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamRepositoryFactory : ICassandraStreamRepositoryFactory
    {
        readonly ICassandraSessionFactory sessionFactory;
        readonly CassandraStorageConfiguration config;
        readonly ICassandraMapperFactory mapperFactory;

        public CassandraStreamRepositoryFactory(ICassandraSessionFactory sessionFactory, ICassandraMapperFactory mapperFactory, CassandraStorageConfiguration config)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.config = config.ThrowIfNull(nameof(config));
            this.mapperFactory = mapperFactory.ThrowIfNull(nameof(mapperFactory));
        }

        public ICassandraStreamRepository Create()
        {
            return new CassandraStreamRepository(sessionFactory, mapperFactory, config);
        }
    }
}
