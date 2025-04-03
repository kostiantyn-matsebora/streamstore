using StreamStore.Multitenancy;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Storage;

namespace StreamStore.Sql.Multitenancy
{
    internal abstract class SqlTenantStreamStorageProvider : ITenantStreamStorageProvider
    {
        readonly ISqlTenantStorageConfigurationProvider configProvider;

        protected SqlTenantStreamStorageProvider(ISqlTenantStorageConfigurationProvider configProvider)
        {
            this.configProvider = configProvider.ThrowIfNull(nameof(configProvider));
        }

        public IStreamStorage GetStorage(Id tenantId)
        {
            tenantId.ThrowIfNull(nameof(tenantId));
            var configuration = configProvider.GetConfiguration(tenantId);
            return new SqlStreamStorage(CreateConnectionFactory(configuration), CreateCommandFactory(configuration), CreateExceptionHandler());
        }

        protected abstract IDbConnectionFactory CreateConnectionFactory(SqlStorageConfiguration configuration);
        protected virtual IDapperCommandFactory CreateCommandFactory(SqlStorageConfiguration configuration)
        {
            return new DefaultDapperCommandFactory(new DefaultSqlQueryProvider(configuration));
        }

        protected virtual ISqlExceptionHandler CreateExceptionHandler()
        {
            return new DefaultSqlExceptionHandler();
        }
    }
}
