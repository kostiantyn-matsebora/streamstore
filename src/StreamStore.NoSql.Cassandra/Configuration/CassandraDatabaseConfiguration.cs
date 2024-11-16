namespace StreamStore.NoSql.Cassandra.Configuration
{
    internal class CassandraDatabaseConfiguration
    {
        public string Keyspace { get; set; } = "streamstore";
        public string EventsTableName { get; set; } = "events";
        public string RevisionPerStreamTableName { get; set; } = "events_revision_per_stream";
        public string EventPerStreamTableName { get; set; } = "events_event_per_stream";
    }
}
