using System.Collections.Concurrent;
using Cassandra.Mapping;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Storage;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraTenantMappingRegistry : ICassandraTenantMappingRegistry
    {
        readonly ICassandraTenantStorageConfigurationProvider configProvider;
        readonly ConcurrentDictionary<Id, MappingConfiguration> registry = new ConcurrentDictionary<Id, MappingConfiguration>();

        public CassandraTenantMappingRegistry(ICassandraTenantStorageConfigurationProvider configProvider)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
        }

        public MappingConfiguration GetMapping(Id tenantId)
        {
           return registry.GetOrAdd(tenantId, CreateMapping);
        }

        private MappingConfiguration CreateMapping(Id tenantId)
        {
            return new MappingConfiguration().Define(new CassandraStreamMapping(configProvider.GetConfiguration(tenantId)));
        }
    }
}
