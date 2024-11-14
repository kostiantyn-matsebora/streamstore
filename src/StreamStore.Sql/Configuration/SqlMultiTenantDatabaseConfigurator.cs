using System;
using Microsoft.Extensions.DependencyInjection;


using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql.Configuration
{
    public class SqlMultiTenantDatabaseConfigurator : SqlDatabaseConfiguratorBase<SqlMultiTenantDatabaseConfigurator>
    {
        Type? connectionStringProviderType;

        readonly SqlDefaultConnectionStringProvider connectionStringProvider = new SqlDefaultConnectionStringProvider();

        public SqlMultiTenantDatabaseConfigurator(IServiceCollection services, SqlDatabaseConfiguration defaultConfig) : base(services, defaultConfig)
        {
        }

        public SqlMultiTenantDatabaseConfigurator WithConnectionStringProvider<TDatabaseProvider>() where TDatabaseProvider : ISqlTenantConnectionStringProvider
        {
            connectionStringProviderType = typeof(TDatabaseProvider);
            return this;
        }

        public SqlMultiTenantDatabaseConfigurator WithConnectionString(Id tenantId, string connectionString)
        {
            connectionStringProvider.AddConnectionString(tenantId, connectionString);
            return this;
        }

        protected override void ApplySpecificDependencies(SqlDatabaseConfiguration configuration, IServiceCollection services)
        {
            if (connectionStringProviderType != null)
            {
                services.AddSingleton(typeof(ISqlTenantConnectionStringProvider), connectionStringProviderType);
            }
            else
            {
                if (!connectionStringProvider.Any)
                {
                    throw new InvalidOperationException("Neither ISqlTenantConnectionStringProvider nor tenant connection strings provided.");
                }
                services.AddSingleton(typeof(ISqlTenantConnectionStringProvider), connectionStringProvider);
            }
             
            services.AddSingleton(typeof(ISqlTenantDatabaseConfigurationProvider), typeof(SqlTenantDatabaseConfigurationProvider));
        }
    }
}
