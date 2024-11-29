using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CosmosDbCqlQueries: CassandraCqlQueries
    {
        public CosmosDbCqlQueries(CassandraStorageConfiguration config): base(config)
        {
        }

        public override Cql StreamActualRevision(string streamId)
        {
            return new Cql($"SELECT revision FROM {config.EventsTableName} WHERE stream_id = ? ORDER BY revision DESC LIMIT 1", streamId);
        }
    }
}
