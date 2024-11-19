using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class DefaultCassandraStorageConfigurationProvider : ICassandraStorageConfigurationProvider
    {
        readonly ICassandraKeyspaceProvider keyspaceProvider;
        private readonly CassandraStorageConfiguration prototype;

        public DefaultCassandraStorageConfigurationProvider(ICassandraKeyspaceProvider keyspaceProvider, CassandraStorageConfiguration prototype)
        {
            this.keyspaceProvider = keyspaceProvider.ThrowIfNull(nameof(keyspaceProvider));
            this.prototype = prototype.ThrowIfNull(nameof(prototype));
        }

        public CassandraStorageConfiguration GetStorageConfiguration(Id tenanId)
        {
            var config = (CassandraStorageConfiguration)prototype.Clone();
            config.Keyspace = keyspaceProvider.GetKeyspace(tenanId);
            return config;
        }
    }
}
