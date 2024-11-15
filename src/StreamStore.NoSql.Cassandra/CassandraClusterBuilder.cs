
using Cassandra;

namespace StreamStore.NoSql.Cassandra
{
    internal class CassandraClusterBuilder
    {
        public Cluster Build()
        {
            return Cluster.Builder().AddContactPoints("localhost").Build();
        }
    }
}
