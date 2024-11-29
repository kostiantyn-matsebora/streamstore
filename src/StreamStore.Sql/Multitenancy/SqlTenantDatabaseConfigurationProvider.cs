using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Multitenancy
{
    internal class SqlTenantDatabaseConfigurationProvider : ISqlTenantDatabaseConfigurationProvider
    {
        readonly SqlDatabaseConfiguration configPrototype;
        readonly ISqlTenantConnectionStringProvider tenantConnectionStringProvider;

        public SqlTenantDatabaseConfigurationProvider(SqlDatabaseConfiguration configPrototype, ISqlTenantConnectionStringProvider connectionStringProvider)
        {
            this.configPrototype = configPrototype.ThrowIfNull(nameof(configPrototype));
            this.tenantConnectionStringProvider = connectionStringProvider.ThrowIfNull(nameof(connectionStringProvider));
        }

        public SqlDatabaseConfiguration GetConfiguration(Id tenantId)
        {
           var tenantConfig = (SqlDatabaseConfiguration)configPrototype.Clone();
           tenantConfig.ConnectionString = tenantConnectionStringProvider.GetConnectionString(tenantId);
           return tenantConfig;
        }
    }
}
