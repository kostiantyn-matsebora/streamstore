using Cassandra;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal interface ICassandraTenantClusterRegistry
    {
        Cluster GetCluster(Id tenantId);
    }
}