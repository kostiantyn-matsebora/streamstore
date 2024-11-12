using StreamStore.Multitenancy;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Database;

namespace StreamStore.Sql.Multitenancy
{
    internal abstract class SqlTenantStreamDatabaseProvider : ITenantStreamDatabaseProvider
    {
        readonly ISqlTenantDatabaseConfigurationProvider configProvider;
        readonly ISqlQueryProvider queryProvider;

        protected SqlTenantStreamDatabaseProvider(ISqlTenantDatabaseConfigurationProvider configProvider, ISqlQueryProvider queryProvider)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
            this.queryProvider = queryProvider.ThrowIfNull(nameof(queryProvider));
        }

        public IStreamDatabase GetDatabase(Id tenantId)
        {
            tenantId.ThrowIfNull(nameof(tenantId));
            return new SqlStreamDatabase(CreateConnectionFactory(configProvider.GetConfiguration(tenantId)), CreateCommandFactory(), CreateExceptionHandler());
        }

        protected abstract IDbConnectionFactory CreateConnectionFactory(SqlDatabaseConfiguration configuration);
        protected virtual IDapperCommandFactory CreateCommandFactory()
        {
            return new DefaultDapperCommandFactory(queryProvider);
        }

        protected virtual ISqlExceptionHandler CreateExceptionHandler()
        {
            return new DefaultSqlExceptionHandler();
        }
    }
}
