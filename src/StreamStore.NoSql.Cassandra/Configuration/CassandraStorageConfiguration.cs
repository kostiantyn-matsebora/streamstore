using System;
using Cassandra;

namespace StreamStore.NoSql.Cassandra.Configuration
{
    public class CassandraStorageConfiguration : ICloneable
    {
        public string Keyspace { get; set; } = "streamstore";
        public string EventsTableName { get; set; } = "events";

        public ConsistencyLevel WriteConsistencyLevel { get; set; } = ConsistencyLevel.All;
        public ConsistencyLevel ReadConsistencyLevel { get; set; } = ConsistencyLevel.All;

        public ConsistencyLevel SerialConsistencyLevel { get; set; } = ConsistencyLevel.Serial;

        public object Clone()
        {
            return new CassandraStorageConfiguration
            {
                Keyspace = Keyspace,
                EventsTableName = EventsTableName,
                WriteConsistencyLevel = WriteConsistencyLevel,
                ReadConsistencyLevel = ReadConsistencyLevel,
                SerialConsistencyLevel = SerialConsistencyLevel
            };
        }
    }
}
