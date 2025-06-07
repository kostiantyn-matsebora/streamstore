using System;
using Cassandra;
using StreamStore.NoSql.Cassandra.Storage;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public class CassandraStorageConfiguration : ICloneable
    {
        public string Keyspace { get; set; } = "streamstore";
        public string EventsTableName { get; set; } = "events";

        public ConsistencyLevel WriteConsistencyLevel { get; set; } = ConsistencyLevel.Quorum;
        public ConsistencyLevel ReadConsistencyLevel { get; set; } = ConsistencyLevel.Quorum;

        public ConsistencyLevel SerialConsistencyLevel { get; set; } = ConsistencyLevel.Serial;

        internal CassandraMode Mode { get; set; } = CassandraMode.Cassandra;

        public object Clone()
        {
            return new CassandraStorageConfiguration
            {
                Keyspace = Keyspace,
                EventsTableName = EventsTableName,
                WriteConsistencyLevel = WriteConsistencyLevel,
                ReadConsistencyLevel = ReadConsistencyLevel,
                SerialConsistencyLevel = SerialConsistencyLevel,
                Mode = Mode
            };
        }
    }
}
