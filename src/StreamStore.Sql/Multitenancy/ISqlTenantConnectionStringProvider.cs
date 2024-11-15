namespace StreamStore.Sql.Multitenancy
{
    public interface ISqlTenantConnectionStringProvider
    {
        string GetConnectionString(Id tenantId);
    }
}
