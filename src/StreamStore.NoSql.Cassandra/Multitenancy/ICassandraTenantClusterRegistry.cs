using Cassandra;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal interface ICassandraTenantClusterRegistry
    {
        ICluster GetCluster(Id tenantId);
    }
}