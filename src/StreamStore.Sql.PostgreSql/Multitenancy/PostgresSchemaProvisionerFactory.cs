using StreamStore.Provisioning;
using StreamStore.Sql.Multitenancy;
using StreamStore.Sql.Provisioning;

namespace StreamStore.Sql.PostgreSql
{
    internal class PostgresSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ISqlTenantDatabaseConfigurationProvider configurationProvider;

        public PostgresSchemaProvisionerFactory(ISqlTenantDatabaseConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider.ThrowIfNull(nameof(configurationProvider));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            var configuration = configurationProvider.GetConfiguration(tenantId);
            return new SqlSchemaProvisioner(new PostgresConnectionFactory(configuration), new PostgresProvisioningQueryProvider(configuration));
        }
    }
}
