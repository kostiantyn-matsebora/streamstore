
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql
{
    public sealed class SqlMultitenancyConfigurator
    {
        readonly IServiceCollection services;
        readonly SqlDefaultConnectionStringProvider connectionStringProvider = new SqlDefaultConnectionStringProvider();

        public SqlMultitenancyConfigurator(IServiceCollection services)
        {
            this.services = services.ThrowIfNull(nameof(services));
            services.AddSingleton<ISqlTenantConnectionStringProvider>(connectionStringProvider);
        }

        public SqlMultitenancyConfigurator WithConnectionStringProvider<TStorageProvider>() where TStorageProvider : class, ISqlTenantConnectionStringProvider
        {
            services.AddSingleton<ISqlTenantConnectionStringProvider, TStorageProvider>();
            return this;

        }

        public SqlMultitenancyConfigurator WithConnectionString(Id tenantId, string connectionString)
        {
            connectionStringProvider.AddConnectionString(tenantId, connectionString);
            return this;
        }
    }
}
