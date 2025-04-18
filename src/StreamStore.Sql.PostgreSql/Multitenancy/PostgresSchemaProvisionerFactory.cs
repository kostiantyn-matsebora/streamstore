﻿using StreamStore.Provisioning;
using StreamStore.Sql.Multitenancy;
using StreamStore.Sql.Provisioning;

namespace StreamStore.Sql.PostgreSql
{
    internal class PostgresSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ISqlTenantStorageConfigurationProvider configurationProvider;

        public PostgresSchemaProvisionerFactory(ISqlTenantStorageConfigurationProvider configurationProvider)
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
