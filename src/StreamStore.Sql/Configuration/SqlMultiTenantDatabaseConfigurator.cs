using System;
using Microsoft.Extensions.DependencyInjection;


using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql.Configuration
{
    public class SqlMultiTenantDatabaseConfigurator : SqlDatabaseConfiguratorBase<SqlMultiTenantDatabaseConfigurator>
    {
        Type? connectionStringProviderType;

        public SqlMultiTenantDatabaseConfigurator(IServiceCollection services, SqlDatabaseConfiguration defaultConfig) : base(services, defaultConfig)
        {
        }

        public SqlMultiTenantDatabaseConfigurator WithConnectionStringProvider<TDatabaseProvider>() where TDatabaseProvider : ISqlTenantConnectionStringProvider
        {
            connectionStringProviderType = typeof(TDatabaseProvider);
            return this;
        }

        protected override void ApplySpecificDependencies(SqlDatabaseConfiguration configuration, IServiceCollection services)
        {
            if (connectionStringProviderType == null)
            {
                throw new InvalidOperationException("ISqlTenantConnectionStringProvider type must be set");
            }
            services.AddSingleton(typeof(ISqlTenantConnectionStringProvider), connectionStringProviderType);
            services.AddSingleton(typeof(ISqlTenantDatabaseConfigurationProvider), typeof(SqlTenantDatabaseConfigurationProvider));
        }
    }
}
