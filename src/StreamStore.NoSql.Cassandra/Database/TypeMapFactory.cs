using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class TypeMapFactory
    {
        readonly CassandraKeyspaceConfiguration config;

        public TypeMapFactory(CassandraKeyspaceConfiguration config)
        {
            this.config = config;
        }

        public Map<EventEntity> CreateEventEntityMap()
        {
            return CreateEventMetadataMap()
                     .Column(e => e.Data, cm => cm.WithName("data"));
        }

        public Map<EventEntity> CreateEventMetadataMap()
        {
            return new Map<EventEntity>()
                     .TableName(config.EventsTableName)
                     .PartitionKey(e => e.StreamId)
                     .Column(e => e.StreamId, cm => cm.WithName("stream_id"))
                     .Column(e => e.Revision, cm => cm.WithName("revision"))
                     .Column(e => e.Id, cm => cm.WithName("id"))
                     .Column(e => e.Timestamp, cm => cm.WithName("timestamp"));
        }

        public Map<RevisionPerStreamEntity> CreateRevisionPerStreamMap()
        {
            return new Map<RevisionPerStreamEntity>()
                     .TableName(config.RevisionPerStreamTableName)
                     .PartitionKey(e => e.StreamId, e => e.Revision)
                     .Column(e => e.StreamId, cm => cm.WithName("stream_id"))
                     .Column(e => e.Revision, cm => cm.WithName("revision"));
        }

        public Map<EventPerStreamEntity> CreateEventPerStreamMap()
        {
            return new Map<EventPerStreamEntity>()
                .TableName(config.EventPerStreamTableName)
                        .PartitionKey(e => e.StreamId, e => e.Id)
                        .Column(e => e.StreamId, cm => cm.WithName("stream_id"))
                        .Column(e => e.Id, cm => cm.WithName("id"));
        }
    }
}
