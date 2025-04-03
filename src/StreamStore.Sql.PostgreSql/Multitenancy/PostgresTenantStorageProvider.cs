using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql.PostgreSql
{
    internal class PostgresTenantStorageProvider : SqlTenantStreamStorageProvider
    {
        public PostgresTenantStorageProvider(ISqlTenantStorageConfigurationProvider configurationProvider) : 
            base(configurationProvider)
        {
        }

        protected override IDbConnectionFactory CreateConnectionFactory(SqlStorageConfiguration configuration)
        {
            return new PostgresConnectionFactory(configuration);
        }

        protected override ISqlExceptionHandler CreateExceptionHandler()
        {
            return new PostgresExceptionHandler();
        }
    }
}
