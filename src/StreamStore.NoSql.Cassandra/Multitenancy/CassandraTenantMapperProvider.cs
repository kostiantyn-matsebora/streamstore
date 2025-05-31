using System.Collections.Concurrent;
using StreamStore.Extensions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Storage;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal interface ICassandraTenantMapperProvider
    {
        public ICassandraMapperProvider GetMapperProvider(Id tenantId);
    }

    internal class CassandraTenantMapperProvider : ICassandraTenantMapperProvider
    {
        readonly ICassandraTenantStorageConfigurationProvider configurationProvider;
        readonly ICassandraTenantClusterRegistry clusterRegistry;
        readonly ICassandraTenantMappingRegistry mappingRegistry;
        readonly ConcurrentDictionary<Id, ICassandraSessionFactory> factories = new ConcurrentDictionary<Id, ICassandraSessionFactory> ();

        public CassandraTenantMapperProvider(
            ICassandraTenantStorageConfigurationProvider configurationProvider,
            ICassandraTenantClusterRegistry clusterRegistry,
            ICassandraTenantMappingRegistry mappingRegistry)
        {
            this.configurationProvider = configurationProvider.ThrowIfNull(nameof(configurationProvider));
            this.clusterRegistry = clusterRegistry.ThrowIfNull(nameof(clusterRegistry));
            this.mappingRegistry = mappingRegistry.ThrowIfNull(nameof(mappingRegistry));
        }

        public ICassandraMapperProvider GetMapperProvider(Id tenantId)
        {
            var mapping = mappingRegistry.GetMapping(tenantId);
            var factory = factories.GetOrAdd(
                tenantId, 
                id => new CassandraSessionFactory(
                    clusterRegistry.GetCluster(id), 
                    configurationProvider.GetConfiguration(id)));

            return new CassandraMapperProvider(factory, mapping);
        }
    }
}
