using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql.PostgreSql
{
    internal class PostgresTenantDatabaseProvider : SqlTenantStreamDatabaseProvider
    {
        public PostgresTenantDatabaseProvider(ISqlTenantDatabaseConfigurationProvider configurationProvider) : 
            base(configurationProvider)
        {
        }

        protected override IDbConnectionFactory CreateConnectionFactory(SqlDatabaseConfiguration configuration)
        {
            return new PostgresConnectionFactory(configuration);
        }

        protected override ISqlExceptionHandler CreateExceptionHandler()
        {
            return new PostgresExceptionHandler();
        }
    }
}
