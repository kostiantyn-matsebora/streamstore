using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql.Sqlite
{
    internal class SqliteTenantDatabaseProvider : SqlTenantStreamDatabaseProvider
    {
        public SqliteTenantDatabaseProvider(ISqlTenantDatabaseConfigurationProvider configurationProvider) :
            base(configurationProvider)
        {
        }

        protected override IDbConnectionFactory CreateConnectionFactory(SqlDatabaseConfiguration configuration)
        {
            return new SqliteDbConnectionFactory(configuration);
        }

        protected override ISqlExceptionHandler CreateExceptionHandler()
        {
            return new SqliteExceptionHandler();
        }
    }
}
