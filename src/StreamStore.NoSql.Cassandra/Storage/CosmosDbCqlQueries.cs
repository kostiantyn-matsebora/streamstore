﻿using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Storage
{
    internal class CosmosDbCqlQueries: CassandraCqlQueries
    {
        public CosmosDbCqlQueries(CassandraStorageConfiguration config): base(config)
        {
        }

        public override Cql StreamMetadata(string streamId)
        {
            return new Cql($"SELECT revision FROM {config.EventsTableName} WHERE stream_id = ? ORDER BY revision DESC LIMIT 1", streamId);
        }
    }
}
