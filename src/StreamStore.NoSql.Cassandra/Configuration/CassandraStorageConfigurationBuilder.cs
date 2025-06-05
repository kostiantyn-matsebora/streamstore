using Cassandra;
using StreamStore.Extensions;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public sealed class CassandraStorageConfigurationBuilder
    {
        readonly CassandraStorageConfiguration config;

        public CassandraStorageConfigurationBuilder(): this(new CassandraStorageConfiguration())
        {
        }

        public CassandraStorageConfigurationBuilder(CassandraStorageConfiguration config)
        {
           this.config = config.ThrowIfNull(nameof(config));
        }
        public CassandraStorageConfigurationBuilder WithKeyspaceName(string keyspace)
        {
            config.Keyspace = keyspace.ThrowIfNullOrEmpty(nameof(keyspace));
            return this;
        }

        public CassandraStorageConfigurationBuilder WithEventsTableName(string eventsTableName)
        {
            config.EventsTableName = eventsTableName.ThrowIfNullOrEmpty(nameof(eventsTableName));
            return this;
        }

        public CassandraStorageConfigurationBuilder WithWriteConsistencyLevel(ConsistencyLevel writeConsistencyLevel)
        {
            config.WriteConsistencyLevel = writeConsistencyLevel;
            return this;
        }

        public CassandraStorageConfigurationBuilder WithReadConsistencyLevel(ConsistencyLevel readConsistencyLevel)
        {
            config.ReadConsistencyLevel = readConsistencyLevel;
            return this;
        }

        public CassandraStorageConfigurationBuilder WithSerialConsistencyLevel(ConsistencyLevel serialConsistencyLevel)
        {
            config.SerialConsistencyLevel = serialConsistencyLevel;
            return this;
        }

        internal CassandraStorageConfiguration Build()
        {
            return config;
        }
    }
}
