namespace StreamStore.Sql.API
{
    public interface ITenantDbConnectionFactory
    {
        IDbConnectionFactory GetConnectionFactory(Id tenantId);
    }
}
