using System.Data.Common;
using Npgsql;
using StreamStore.SQL;

namespace StreamStore.Sql.Postgres
{
    internal class PostgresConnectionFactory : IDbConnectionFactory
    {
        readonly SqlDatabaseConfiguration configuration;

        public PostgresConnectionFactory(SqlDatabaseConfiguration configuration)
        {
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }
        public DbConnection GetConnection()
        {
            return new NpgsqlConnection(configuration.ConnectionString);
        }
    }
}
