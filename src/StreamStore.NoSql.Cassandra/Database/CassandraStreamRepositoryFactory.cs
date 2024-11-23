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
        private readonly MappingConfiguration mapping;

        public CassandraStreamRepositoryFactory(ICassandraSessionFactory sessionFactory, CassandraStorageConfiguration config, MappingConfiguration mapping)
        {
            this.sessionFactory = sessionFactory.ThrowIfNull(nameof(sessionFactory));
            this.config = config.ThrowIfNull(nameof(config));
            this.mapping = mapping.ThrowIfNull(nameof(mapping));
        }

        public ICassandraStreamRepository Create()
        {
            return new CassandraStreamRepository(sessionFactory, config, mapping);
        }
    }
}
