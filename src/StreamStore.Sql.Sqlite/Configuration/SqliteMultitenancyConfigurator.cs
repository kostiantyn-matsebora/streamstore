
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql.Sqlite.Configuration
{
    public sealed class SqliteMultitenancyConfigurator
    {
        readonly IServiceCollection services;
        readonly SqlDefaultConnectionStringProvider connectionStringProvider = new SqlDefaultConnectionStringProvider();

        public SqliteMultitenancyConfigurator(IServiceCollection services)
        {
            this.services = services.ThrowIfNull(nameof(services));
            services.AddSingleton<ISqlTenantConnectionStringProvider>(connectionStringProvider);
        }

        public SqliteMultitenancyConfigurator WithConnectionStringProvider<TStorageProvider>() where TStorageProvider : class, ISqlTenantConnectionStringProvider
        {
            services.AddSingleton<ISqlTenantConnectionStringProvider, TStorageProvider>();
            return this;

        }

        public SqliteMultitenancyConfigurator WithConnectionString(Id tenantId, string connectionString)
        {
            connectionStringProvider.AddConnectionString(tenantId, connectionString);
            return this;
        }
    }
}
