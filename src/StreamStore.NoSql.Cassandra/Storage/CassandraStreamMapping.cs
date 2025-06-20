﻿using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Models;

namespace StreamStore.NoSql.Cassandra.Storage
{
    internal class CassandraStreamMapping: Mappings
    {
        public CassandraStreamMapping(CassandraStorageConfiguration config)
        {
            For<EventEntity>()
                .TableName(config.EventsTableName)
                     .PartitionKey(e => e.StreamId)
                     .ClusteringKey(e => e.Revision)
                     .Column(e => e.StreamId, cm => cm.WithName("stream_id"))
                     .Column(e => e.Revision, cm => cm.WithName("revision"))
                     .Column(e => e.Id, cm => cm.WithName("id"))
                     .Column(e => e.Timestamp, cm => cm.WithName("timestamp"))
                     .Column(e => e.Data, cm => cm.WithName("data"))
                     .Column(e => e.CustomProperties, cm => cm.WithName("custom_properties"));
            For<EventMetadataEntity>()
                .TableName(config.EventsTableName)
                     .PartitionKey(e => e.StreamId)
                     .ClusteringKey(e => e.Revision)
                     .Column(e => e.StreamId, cm => cm.WithName("stream_id"))
                     .Column(e => e.Revision, cm => cm.WithName("revision"))
                     .Column(e => e.Id, cm => cm.WithName("id"))
                     .Column(e => e.Timestamp, cm => cm.WithName("timestamp"));
            For<RevisionStreamEntity>()
                  .TableName(config.EventsTableName)
                     .PartitionKey(e => e.StreamId)
                     .ClusteringKey(e => e.Revision)
                     .Column(e => e.StreamId, cm => cm.WithName("stream_id"))
                     .Column(e => e.Revision, cm => cm.WithName("revision"));
        }
    }
}
