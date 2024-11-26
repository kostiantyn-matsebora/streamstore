using Cassandra;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    interface ITenantClusterConfigurator
    {
        void Configure(Id tenantId, Builder builder);
    }
}
