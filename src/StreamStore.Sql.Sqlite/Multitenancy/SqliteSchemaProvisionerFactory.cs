using StreamStore.Provisioning;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;

using StreamStore.Sql.Provisioning;
using StreamStore.Sql.Sqlite.Provisioning;

namespace StreamStore.Sql.Sqlite
{
    internal class SqliteSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ISqlTenantStorageConfigurationProvider configurationProvider;
        readonly MigrationConfiguration migrationConfig;

        public SqliteSchemaProvisionerFactory(ISqlTenantStorageConfigurationProvider configurationProvider, MigrationConfiguration migrationConfig)
        {
            this.configurationProvider = configurationProvider.ThrowIfNull(nameof(configurationProvider));
            this.migrationConfig = migrationConfig.ThrowIfNull(nameof(migrationConfig));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            var storageConfig = configurationProvider.GetConfiguration(tenantId);
            return new SqlSchemaProvisioner(new SqliteMigrator(storageConfig, migrationConfig));
        }
    }
}
