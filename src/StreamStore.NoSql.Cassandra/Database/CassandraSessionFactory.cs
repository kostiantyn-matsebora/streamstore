using Cassandra;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;


namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraSessionFactory : ICassandraSessionFactory
    {

        readonly Cluster cluster;
        readonly CassandraStorageConfiguration config;

        public CassandraSessionFactory(Cluster cluster, CassandraStorageConfiguration config)
        {
            this.cluster = cluster;
            this.config = config.ThrowIfNull(nameof(config));
        }

        public ISession Open()
        {
            var session = cluster.Connect(config.Keyspace);
            return cluster.Connect(config.Keyspace);
        }
    }
}
