using Cassandra;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    public class CassandraTestDatabase : ITestDatabase
    {
        readonly Cluster cluster;
        readonly CassandraKeyspaceConfiguration config;
        public readonly string Keyspace;

        public CassandraTestDatabase(string keyspace, Action<Builder>? configureCluster = null)
        {
            Keyspace = keyspace;
            config = new CassandraKeyspaceConfigurationBuilder().WithKeyspaceName(keyspace).Build();
            var configurator = configureCluster ?? ConfigureCluster;
            var builder = Cluster.Builder();
            configurator(builder);
            cluster = builder.Build();
        }

        public bool EnsureExists()
        {
            try
            {
                using (var session = cluster.Connect())
                {
                    session.Execute($"CREATE KEYSPACE IF NOT EXISTS {config.Keyspace} WITH REPLICATION = {{ 'class' : 'SimpleStrategy', 'replication_factor' : 1 }};");
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        void ConfigureCluster(Builder builder)
        {
            builder.AddContactPoint("localhost");
        }
    }
}
