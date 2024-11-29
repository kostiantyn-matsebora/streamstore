using StreamStore.NoSql.Cassandra.Configuration;

namespace StreamStore.NoSql.Cassandra.Database
{
    internal interface ICassandraCqlQueriesProvider
    {
        ICassandraCqlQueries GetCqlQueries(CassandraStorageConfiguration config);
    }

    internal class CassandraCqlQueriesProvider : ICassandraCqlQueriesProvider
    {
        readonly CassandraMode mode;

        public CassandraCqlQueriesProvider(CassandraMode mode)
        {
            this.mode = mode;
        }

        public ICassandraCqlQueries GetCqlQueries(CassandraStorageConfiguration config)
        {
            if (mode == CassandraMode.Cassandra)
            {
                return new CassandraCqlQueries(config);
            }
            return new CosmosDbCqlQueries(config);
        }
    }
}
