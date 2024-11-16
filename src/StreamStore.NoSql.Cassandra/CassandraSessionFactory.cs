using Cassandra;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;

namespace StreamStore.NoSql.Cassandra
{
    internal class CassandraSessionFactory : ICassandraSessionFactory
    {
        readonly CassandraClusterBuilder clusterBuilder;
        readonly CassandraDatabaseConfiguration dbConfig;

        public CassandraSessionFactory(CassandraClusterBuilder clusterBuilder, CassandraDatabaseConfiguration dbConfig)
        {
            this.clusterBuilder = clusterBuilder.ThrowIfNull(nameof(clusterBuilder));
            this.dbConfig = dbConfig.ThrowIfNull(nameof(dbConfig));
        }

        public ISession CreateSession()
        {
            var cluster = clusterBuilder.Build();
            var session =  cluster.Connect(dbConfig.Keyspace);
            return session;
        }
    }
}
