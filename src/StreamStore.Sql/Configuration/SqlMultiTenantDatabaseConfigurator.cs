using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Multitenancy;

using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql.Configuration
{
    internal class SqlMultiTenantDatabaseConfigurator : SqlDatabaseConfiguratorBase<SqlMultiTenantDatabaseConfigurator>
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

        protected override IServiceCollection ApplyModeSpecificDependencies(SqlDatabaseConfiguration configuration, IServiceCollection services)
        {
           services.AddSingleton(typeof(ISqlTenantConnectionStringProvider), connectionStringProviderType!);
           services.AddSingleton(typeof(ISqlTenantDatabaseConfigurationProvider), typeof(SqlTenantDatabaseConfigurationProvider));
           return services;
        }
    }
}
