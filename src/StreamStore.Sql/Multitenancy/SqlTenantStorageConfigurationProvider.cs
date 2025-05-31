using StreamStore.Extensions;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.Multitenancy
{
    internal class SqlTenantStorageConfigurationProvider : ISqlTenantStorageConfigurationProvider
    {
        readonly SqlStorageConfiguration configPrototype;
        readonly ISqlTenantConnectionStringProvider tenantConnectionStringProvider;

        public SqlTenantStorageConfigurationProvider(SqlStorageConfiguration configPrototype, ISqlTenantConnectionStringProvider connectionStringProvider)
        {
            this.configPrototype = configPrototype.ThrowIfNull(nameof(configPrototype));
            this.tenantConnectionStringProvider = connectionStringProvider.ThrowIfNull(nameof(connectionStringProvider));
        }

        public SqlStorageConfiguration GetConfiguration(Id tenantId)
        {
           var tenantConfig = (SqlStorageConfiguration)configPrototype.Clone();
           tenantConfig.ConnectionString = tenantConnectionStringProvider.GetConnectionString(tenantId);
           return tenantConfig;
        }
    }
}
