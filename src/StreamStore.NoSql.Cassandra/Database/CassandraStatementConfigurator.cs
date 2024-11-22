using Cassandra;
using Cassandra.Data.Linq;
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
        public TStatement ConfigureQuery<TStatement>(IStatement statement) where TStatement : IStatement
        {
            return (TStatement)statement
                .SetConsistencyLevel(config.ReadConsistencyLevel)
                .SetSerialConsistencyLevel(config.SerialConsistencyLevel);
        }

        public CqlQueryOptions ConfigureInsert(CqlQueryOptions options)
        {
            return options
                .SetConsistencyLevel(config.WriteConsistencyLevel)
                .SetSerialConsistencyLevel(config.SerialConsistencyLevel);
        }

    }
}
