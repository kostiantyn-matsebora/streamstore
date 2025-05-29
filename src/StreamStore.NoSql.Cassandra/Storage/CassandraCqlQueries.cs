using System.Collections.Generic;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Storage
{
    internal class CassandraCqlQueries: ICassandraCqlQueries
    {
        protected readonly CassandraStorageConfiguration config;

        public CassandraCqlQueries(CassandraStorageConfiguration config)
        {
            this.config = config.ThrowIfNull(nameof(config));
        }
        public virtual Cql StreamMetadata(string streamId)
        {
            return new Cql($"SELECT id, stream_id, revision, timestamp FROM { config.EventsTableName } WHERE stream_id = ? ORDER BY revision DESC LIMIT 1", streamId);
        }

        public Cql DeleteStream(string streamId)
        {
            return new Cql($"DELETE FROM {config.EventsTableName} WHERE stream_id = ?", streamId);
        }

        public Cql StreamEvents(string streamId, int from, int count)
        {
            return new Cql($"SELECT id, stream_id, timestamp, revision, data, custom_properties FROM {config.EventsTableName} WHERE stream_id = ? AND revision >= ? LIMIT ?", streamId, from, count);
        }
    }
}
