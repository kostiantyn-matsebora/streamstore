using Cassandra;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStreamRepositoryFactory : ICassandraStreamRepositoryFactory
    {
        readonly ICassandraSessionFactory sessionFactory;
        readonly ICassandraPredicateProvider predicateProvider;
        readonly CassandraStorageConfiguration config;

        public CassandraStreamRepositoryFactory(ICassandraSessionFactory sessionFactory, ICassandraPredicateProvider predicateProvider, CassandraStorageConfiguration config)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.predicateProvider = predicateProvider.ThrowIfNull(nameof(predicateProvider));
            this.config = config.ThrowIfNull(nameof(config));
        }

        public ICassandraStreamRepository Create()
        {
            return new CassandraStreamRepository(sessionFactory, predicateProvider, config);
        }
    }
}
