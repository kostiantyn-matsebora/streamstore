using StreamStore.Multitenancy;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Database;

namespace StreamStore.Sql.Multitenancy
{
    internal abstract class SqlTenantStreamDatabaseProvider : ITenantStreamDatabaseProvider
    {
        readonly ISqlTenantDatabaseConfigurationProvider configProvider;

        protected SqlTenantStreamDatabaseProvider(ISqlTenantDatabaseConfigurationProvider configProvider)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
        }

        public IStreamStorage GetDatabase(Id tenantId)
        {
            tenantId.ThrowIfNull(nameof(tenantId));
            var configuration = configProvider.GetConfiguration(tenantId);
            return new SqlStreamDatabase(CreateConnectionFactory(configuration), CreateCommandFactory(configuration), CreateExceptionHandler());
        }

        protected abstract IDbConnectionFactory CreateConnectionFactory(SqlDatabaseConfiguration configuration);
        protected virtual IDapperCommandFactory CreateCommandFactory(SqlDatabaseConfiguration configuration)
        {
            return new DefaultDapperCommandFactory(new DefaultSqlQueryProvider(configuration));
        }

        protected virtual ISqlExceptionHandler CreateExceptionHandler()
        {
            return new DefaultSqlExceptionHandler();
        }
    }
}
