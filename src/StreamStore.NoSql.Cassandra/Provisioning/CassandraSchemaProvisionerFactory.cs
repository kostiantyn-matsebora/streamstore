using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Provisioning;

namespace StreamStore.NoSql.Cassandra.Provisioning
{
    internal class CassandraSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        readonly ICassandraTenantStorageConfigurationProvider configProvider;
        readonly ICassandraTenantMapperProvider mapperProvider;

        public CassandraSchemaProvisionerFactory(ICassandraTenantStorageConfigurationProvider configProvider, ICassandraTenantMapperProvider mapperProvider)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.mapperProvider = mapperProvider.ThrowIfNull(nameof(mapperProvider));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            return new CassandraSchemaProvisioner(mapperProvider.GetMapperProvider(tenantId), configProvider.GetConfiguration(tenantId));
        }
    }
}
