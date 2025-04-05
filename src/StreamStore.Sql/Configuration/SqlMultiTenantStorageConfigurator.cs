using System;
using Microsoft.Extensions.DependencyInjection;


using StreamStore.Sql.Multitenancy;

namespace StreamStore.Sql.Configuration
{
    public class SqlMultiTenantStorageConfigurator : SqlStorageConfiguratorBase<SqlMultiTenantStorageConfigurator>
    {
        Type? connectionStringProviderType;

        readonly SqlDefaultConnectionStringProvider connectionStringProvider = new SqlDefaultConnectionStringProvider();

        public SqlMultiTenantStorageConfigurator(IServiceCollection services, SqlStorageConfiguration defaultConfig) : base(services, defaultConfig)
        {
        }

        public SqlMultiTenantStorageConfigurator WithConnectionStringProvider<TStorageProvider>() where TStorageProvider : ISqlTenantConnectionStringProvider
        {
            connectionStringProviderType = typeof(TStorageProvider);
            return this;
        }

        public SqlMultiTenantStorageConfigurator WithConnectionString(Id tenantId, string connectionString)
        {
            connectionStringProvider.AddConnectionString(tenantId, connectionString);
            return this;
        }

        protected override void ApplySpecificDependencies(SqlStorageConfiguration configuration, IServiceCollection services)
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
             
            services.AddSingleton(typeof(ISqlTenantStorageConfigurationProvider), typeof(SqlTenantStorageConfigurationProvider));
        }
    }
}
