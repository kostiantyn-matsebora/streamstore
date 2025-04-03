using StreamStore.Provisioning;
using StreamStore.Sql.Multitenancy;

using StreamStore.Sql.Provisioning;

namespace StreamStore.Sql.Sqlite
{
    internal class SqliteSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ISqlTenantStorageConfigurationProvider configurationProvider;

        public SqliteSchemaProvisionerFactory(ISqlTenantStorageConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider.ThrowIfNull(nameof(configurationProvider));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            var configuration = configurationProvider.GetConfiguration(tenantId);
            return new SqlSchemaProvisioner(
                new SqliteDbConnectionFactory(configuration), 
                new SqliteProvisioningQueryProvider(configuration));
        }
    }
}
