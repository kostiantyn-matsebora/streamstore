using System;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public class CassandraKeyspaceConfiguration : ICloneable
    {
        public string Keyspace { get; set; } = "streamstore";
        public string EventsTableName { get; set; } = "events";
        public string RevisionPerStreamTableName { get; set; } = "events_revision_per_stream";
        public string EventPerStreamTableName { get; set; } = "events_event_per_stream";

        public object Clone()
        {
            return new CassandraKeyspaceConfiguration
            {
                Keyspace = Keyspace,
                EventsTableName = EventsTableName,
                RevisionPerStreamTableName = RevisionPerStreamTableName,
                EventPerStreamTableName = EventPerStreamTableName
            };
        }
    }
}
