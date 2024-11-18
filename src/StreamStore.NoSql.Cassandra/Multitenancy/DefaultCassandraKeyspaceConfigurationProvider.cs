using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Multitenancy
{
    internal class DefaultCassandraKeyspaceConfigurationProvider : ICassandraKeyspaceConfigurationProvider
    {
        readonly ICassandraKeyspaceProvider keyspaceProvider;
        private readonly CassandraKeyspaceConfiguration prototype;

        public DefaultCassandraKeyspaceConfigurationProvider(ICassandraKeyspaceProvider keyspaceProvider, CassandraKeyspaceConfiguration prototype)
        {
            this.keyspaceProvider = keyspaceProvider.ThrowIfNull(nameof(keyspaceProvider));  
            this.prototype = prototype.ThrowIfNull(nameof(prototype));
        }

        

        public CassandraKeyspaceConfiguration GetKeyspaceConfiguration(Id tenanId)
        {
            var config = (CassandraKeyspaceConfiguration)prototype.Clone();
            config.Keyspace = keyspaceProvider.GetKeyspace(tenanId);
            return config;
        }
    }
}
