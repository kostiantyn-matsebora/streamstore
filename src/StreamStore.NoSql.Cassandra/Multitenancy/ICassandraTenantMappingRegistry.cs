using Cassandra.Mapping;


namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal interface ICassandraTenantMappingRegistry
    {
        MappingConfiguration GetMapping(Id tenantId);
    }
}
