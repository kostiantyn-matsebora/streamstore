using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Multitenancy
{
    public interface ISqlTenantDatabaseConfigurationProvider
    {
        SqlDatabaseConfiguration GetConfiguration(Id tenantId);
    }
}
