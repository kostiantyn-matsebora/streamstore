using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class TypeMapFactory
    {
        readonly CassandraStorageConfiguration config;

        public TypeMapFactory(CassandraStorageConfiguration config)
        {
            this.config = config;
        }

        public Map<EventEntity> CreateEventEntityMap()
        {
            return new Map<EventEntity>()
                     .TableName(config.EventsTableName)
                     .PartitionKey(e => e.StreamId)
                     .ClusteringKey(e => e.Revision)
                     .Column(e => e.StreamId, cm => cm.WithName("stream_id"))
                     .Column(e => e.Revision, cm => cm.WithName("revision"))
                     .Column(e => e.Id, cm => cm.WithName("id"))
                     .Column(e => e.Timestamp, cm => cm.WithName("timestamp"))
                     .Column(e => e.Data, cm => cm.WithName("data"));
        }

        public Map<EventMetadataEntity> CreateEventMetadataMap()
        {
            return new Map<EventMetadataEntity>()
                     .TableName(config.EventsTableName)
                     .PartitionKey(e => e.StreamId)
                     .ClusteringKey(e => e.Revision)
                     .Column(e => e.StreamId, cm => cm.WithName("stream_id"))
                     .Column(e => e.Revision, cm => cm.WithName("revision"))
                     .Column(e => e.Id, cm => cm.WithName("id"))
                     .Column(e => e.Timestamp, cm => cm.WithName("timestamp"));
        }

        public Map<RevisionStreamEntity> CreateStreamRevisionMap()
        {
            return new Map<RevisionStreamEntity>()
                     .TableName(config.EventsTableName)
                     .PartitionKey(e => e.StreamId)
                     .ClusteringKey(e => e.Revision)
                     .Column(e => e.StreamId, cm => cm.WithName("stream_id"))
                     .Column(e => e.Revision, cm => cm.WithName("revision"));
        }
    }
}
