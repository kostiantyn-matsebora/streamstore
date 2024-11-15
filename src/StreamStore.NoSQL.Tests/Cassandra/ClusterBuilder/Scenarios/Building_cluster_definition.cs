using StreamStore.NoSql.Cassandra;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.ClusterBuilder
{
    public class Building_cluster_definition: Scenario
    {
        [Fact]
        public void Build_cluster()
        {
            var builder = new CassandraClusterBuilder();
            var cluster = builder.Build();
            
            var session = cluster.Connect();
            session.Execute("CREATE KEYSPACE IF NOT EXISTS test WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : 1 };");

            session.Execute("CREATE TABLE IF NOT EXISTS test.streams (stream_id uuid, stream_version int, message_id timeuuid, type text, position int, data blob, metadata blob, PRIMARY KEY (stream_id, stream_version));");
        }
    }
}