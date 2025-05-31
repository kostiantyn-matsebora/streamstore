using StreamStore.Extensions;
using StreamStore.Provisioning;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;
using StreamStore.Sql.PostgreSql.Provisioning;
using StreamStore.Sql.Provisioning;

namespace StreamStore.Sql.PostgreSql
{
    internal class PostgresSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ISqlTenantStorageConfigurationProvider configurationProvider;
        readonly MigrationConfiguration migrationConfig;

        public PostgresSchemaProvisionerFactory(ISqlTenantStorageConfigurationProvider configurationProvider, MigrationConfiguration migrationConfig)
        {
            this.configurationProvider = configurationProvider.ThrowIfNull(nameof(configurationProvider));
            this.migrationConfig = migrationConfig.ThrowIfNull(nameof(migrationConfig));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            var storageConfig = configurationProvider.GetConfiguration(tenantId);
            return new SqlSchemaProvisioner(new PostgreSqlMigrator(storageConfig, migrationConfig));
        }
    }
}
