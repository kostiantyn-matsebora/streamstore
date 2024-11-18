using System.CodeDom;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public sealed class CassandraKeyspaceConfigurationBuilder
    {
        readonly CassandraKeyspaceConfiguration config = new CassandraKeyspaceConfiguration() { Keyspace = "streamstore" };

        public CassandraKeyspaceConfigurationBuilder WithKeyspaceName(string keyspace)
        {
            config.Keyspace = keyspace.ThrowIfNullOrEmpty(nameof(keyspace));
            return this;
        }

        public CassandraKeyspaceConfigurationBuilder WithEventsTableName(string eventsTableName)
        {
            config.EventsTableName = eventsTableName.ThrowIfNullOrEmpty(nameof(eventsTableName));
            return this;
        }

        public CassandraKeyspaceConfigurationBuilder WithRevisionPerStreamTableName(string revisionPerStreamTableName)
        {
            config.RevisionPerStreamTableName = revisionPerStreamTableName.ThrowIfNullOrEmpty(nameof(revisionPerStreamTableName));
            return this;
        }

        public CassandraKeyspaceConfigurationBuilder WithEventPerStreamTableName(string eventPerStreamTableName)
        {
            config.EventPerStreamTableName = eventPerStreamTableName.ThrowIfNullOrEmpty(nameof(eventPerStreamTableName));
            return this;
        }

        internal CassandraKeyspaceConfiguration Build()
        {
            return config;
        }
    }
}
