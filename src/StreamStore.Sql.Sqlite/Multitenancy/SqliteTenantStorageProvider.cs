using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql.Sqlite
{
    internal class SqliteTenantStorageProvider : SqlTenantStreamStorageProvider
    {
        public SqliteTenantStorageProvider(ISqlTenantStorageConfigurationProvider configurationProvider) :
            base(configurationProvider)
        {
        }

        protected override IDbConnectionFactory CreateConnectionFactory(SqlStorageConfiguration configuration)
        {
            return new SqliteDbConnectionFactory(configuration);
        }

        protected override ISqlExceptionHandler CreateExceptionHandler()
        {
            return new SqliteExceptionHandler();
        }
    }
}
