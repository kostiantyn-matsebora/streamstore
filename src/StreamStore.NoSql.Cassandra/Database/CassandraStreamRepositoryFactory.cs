using Cassandra;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamRepositoryFactory : ICassandraStreamRepositoryFactory
    {
        readonly ICassandraSessionFactory sessionFactory;
        private readonly CassandraStorageConfiguration config;

        public CassandraStreamRepositoryFactory(ICassandraSessionFactory sessionFactory, CassandraStorageConfiguration config)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.config = config.ThrowIfNull(nameof(config));
        }

        public ICassandraStreamRepository Create()
        {
            return new CassandraStreamRepository(sessionFactory, config);
        }
    }
}
