using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraCqlQueries
    {
        readonly CassandraStorageConfiguration config;

        public CassandraCqlQueries(CassandraStorageConfiguration config)
        {
            this.config = config.ThrowIfNull(nameof(config));
        }
        public Cql StreamActualRevision(string streamId)
        {
            return new Cql($"SELECT MAX(revision) FROM {config.EventsTableName} WHERE stream_id = ?", streamId);
        }

        public Cql DeleteStream(string streamId)
        {
            return new Cql($"DELETE FROM {config.EventsTableName} WHERE stream_id = ?", streamId);
        }

        public Cql StreamMetadata(string streamId)
        {
            return new Cql($"SELECT id, stream_id, timestamp, revision  FROM {config.EventsTableName} WHERE stream_id = ?", streamId);
        }

        public Cql StreamEvents(string streamId, int from, int count)
        {
            return new Cql($"SELECT id, stream_id, timestamp, revision, data FROM {config.EventsTableName} WHERE stream_id = ? AND revision >= ? LIMIT ?", streamId, from, count);
        }
    }
}
