using Cassandra;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class DataContextFactory
    {
        readonly TypeMapFactory mapFactory;
        readonly ICassandraSessionFactory sessionFactory;
        private readonly CassandraStorageConfiguration config;

        public DataContextFactory(TypeMapFactory mapFactory, ICassandraSessionFactory sessionFactory, CassandraStorageConfiguration config)
        {
          this.mapFactory = mapFactory.ThrowIfNull(nameof(mapFactory));
          this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
          this.config = config.ThrowIfNull(nameof(config));
        }

        public DataContext Create()
        {
            return new DataContext(mapFactory, sessionFactory, config);
        }
    }
}
