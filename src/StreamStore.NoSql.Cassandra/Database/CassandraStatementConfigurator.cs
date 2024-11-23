using Cassandra;
using Cassandra.Mapping;
using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal class CassandraStatementConfigurator
    {
        readonly CassandraStorageConfiguration config;

        public CassandraStatementConfigurator(CassandraStorageConfiguration config)
        {
           this.config = config.ThrowIfNull(nameof(config));
        }
        public TStatement Query<TStatement>(IStatement statement) where TStatement : IStatement
        {
            return (TStatement)statement
                .SetConsistencyLevel(config.ReadConsistencyLevel)
                .SetSerialConsistencyLevel(config.SerialConsistencyLevel);
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
