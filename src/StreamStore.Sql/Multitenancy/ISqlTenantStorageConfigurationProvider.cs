using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Multitenancy
{
    public interface ISqlTenantStorageConfigurationProvider
    {
        SqlStorageConfiguration GetConfiguration(Id tenantId);
    }
}
