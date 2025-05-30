using System.Data.Common;
using Npgsql;
using StreamStore.Extensions;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.PostgreSql
{
    internal class PostgresConnectionFactory : IDbConnectionFactory
    {
        readonly SqlStorageConfiguration configuration;

        public PostgresConnectionFactory(SqlStorageConfiguration configuration)
        {
            this.configuration = configuration.ThrowIfNull(nameof(configuration));
        }
        public DbConnection GetConnection()
        {
            return new NpgsqlConnection(configuration.ConnectionString);
        }
    }
}
