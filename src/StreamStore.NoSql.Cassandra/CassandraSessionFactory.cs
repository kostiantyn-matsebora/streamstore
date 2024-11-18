using Cassandra;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;


namespace StreamStore.NoSql.Cassandra
{
    internal class CassandraSessionFactory : ICassandraSessionFactory
    {
        
        readonly Cluster cluster;
        readonly CassandraKeyspaceConfiguration config;

        public CassandraSessionFactory(Cluster cluster, CassandraKeyspaceConfiguration config)
        {
            this.cluster = cluster;
            this.config = config.ThrowIfNull(nameof(config));
        }

        public ISession CreateSession()
        {
           return cluster.Connect(config.Keyspace);
        }
    }
}
