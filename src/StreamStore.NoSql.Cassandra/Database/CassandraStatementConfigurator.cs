using Cassandra.Data.Linq;
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
        public CqlQuery<TEntity> ConfigureQuery<TEntity>(CqlQuery<TEntity> query)
        {
            return query
                .SetConsistencyLevel(config.ReadConsistencyLevel)
                .SetSerialConsistencyLevel(config.SerialConsistencyLevel);
        }

        public CqlCommand ConfigureInsert(CqlCommand command)
        {
            return command
                .SetConsistencyLevel(config.WriteConsistencyLevel)
                .SetSerialConsistencyLevel(config.SerialConsistencyLevel);
        }
    }
}
