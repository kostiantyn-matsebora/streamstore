using Cassandra;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Storage
{
    internal class CassandraStatementConfigurator
    {
        readonly CassandraStorageConfiguration config;

        public CassandraStatementConfigurator(CassandraStorageConfiguration config)
        {
           this.config = config.ThrowIfNull(nameof(config));
        }
        public Cql Query(Cql cql) 
        {
            return cql
                  .WithOptions(o =>
                    o.SetConsistencyLevel(config.ReadConsistencyLevel)
                     .SetSerialConsistencyLevel(config.SerialConsistencyLevel));
        }

        public ICqlBatch Batch(ICqlBatch batch)
        {
            return batch
                .WithOptions(o =>
                    o.SetConsistencyLevel(config.WriteConsistencyLevel)
                     .SetSerialConsistencyLevel(config.SerialConsistencyLevel));
        }
    }
}
