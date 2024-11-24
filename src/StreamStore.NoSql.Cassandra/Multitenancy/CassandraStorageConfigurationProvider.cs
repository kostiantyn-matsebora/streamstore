using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class CassandraStorageConfigurationProvider : ICassandraTenantStorageConfigurationProvider
    {
        readonly ICassandraKeyspaceProvider keyspaceProvider;
        private readonly CassandraStorageConfiguration prototype;

        public CassandraStorageConfigurationProvider(ICassandraKeyspaceProvider keyspaceProvider, CassandraStorageConfiguration prototype)
        {
            this.keyspaceProvider = keyspaceProvider.ThrowIfNull(nameof(keyspaceProvider));
            this.prototype = prototype.ThrowIfNull(nameof(prototype));
        }

        public CassandraStorageConfiguration GetConfiguration(Id tenanId)
        {
            var config = (CassandraStorageConfiguration)prototype.Clone();
            config.Keyspace = keyspaceProvider.GetKeyspace(tenanId);
            return config;
        }
    }
}
