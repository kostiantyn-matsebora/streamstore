using Cassandra.Data.Linq;
using StreamStore.NoSql.Cassandra;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.Testing.Framework;

namespace StreamStore.NoSql.Tests.Cassandra
{
    internal class CassandraTestDatabase : ITestDatabase
    {
        readonly CassandraClusterBuilder clusterBuilder;

        readonly CassandraDatabaseConfiguration configuration;

        public CassandraTestDatabase(CassandraClusterBuilder clusterBuilder, CassandraDatabaseConfiguration configuration)
        {
            this.clusterBuilder = clusterBuilder.ThrowIfNull(nameof(clusterBuilder));
             this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }

        public bool EnsureExists()
        {
            var cluster = clusterBuilder.Build();
            using (var session = cluster.Connect())
            {
                session.Execute($"CREATE KEYSPACE IF NOT EXISTS {configuration.Keyspace} WITH REPLICATION = {{ 'class' : 'SimpleStrategy', 'replication_factor' : 1 }};");
            }
            return false;
        }
    }
}
