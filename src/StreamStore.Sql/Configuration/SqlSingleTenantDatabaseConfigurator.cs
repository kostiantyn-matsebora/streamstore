using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using StreamStore.Sql.API;
using StreamStore.Sql.Database;
using StreamStore.Sql.Provisioning;



namespace StreamStore.Sql.Configuration
{
    public sealed class SqlSingleTenantDatabaseConfigurator: SqlDatabaseConfiguratorBase<SqlSingleTenantDatabaseConfigurator>
    {
        Type? connectionFactoryType;
        Type sqlQueryProviderType = typeof(DefaultSqlQueryProvider);
        Type? sqlProvisionQueryProviderType;
        Type commandFactoryType = typeof(DefaultDapperCommandFactory);
        public SqlSingleTenantDatabaseConfigurator(IServiceCollection services, SqlDatabaseConfiguration defaultConfig): base(services, defaultConfig)
        {
        }


        public SqlSingleTenantDatabaseConfigurator WithConnectionFactory<TFactory>() where TFactory : IDbConnectionFactory
        {
            connectionFactoryType = typeof(TFactory);
            return this;
        }

        public SqlSingleTenantDatabaseConfigurator WithQueryProvider<TProvider>() where TProvider : ISqlQueryProvider
        {
            sqlQueryProviderType = typeof(TProvider);
            return this;
        }

        public SqlSingleTenantDatabaseConfigurator WithProvisioingQueryProvider<TProvisioningProvider>() where TProvisioningProvider : ISqlProvisioningQueryProvider
        {
            sqlProvisionQueryProviderType = typeof(TProvisioningProvider);
            return this;
        }

        public SqlSingleTenantDatabaseConfigurator WithCommandFactory<TFactory>() where TFactory : IDapperCommandFactory
        {
            commandFactoryType = typeof(TFactory);
            return this;
        }

        protected override void ApplySpecificDependencies(SqlDatabaseConfiguration configuration, IServiceCollection services)
        {
            if (connectionFactoryType == null)
                throw new InvalidOperationException("IDbConnectionFactory type not set");

            if (sqlProvisionQueryProviderType == null)
                throw new InvalidOperationException("ISqlProvisionQueryProvider type not set");

            services.AddSingleton(typeof(IDbConnectionFactory), connectionFactoryType);
            services.AddSingleton(typeof(ISqlQueryProvider), sqlQueryProviderType);
            services.AddSingleton(typeof(ISqlProvisioningQueryProvider), sqlProvisionQueryProviderType);
            services.AddSingleton<ISchemaProvisioner, SqlSchemaProvisioner>();
            services.AddSingleton(typeof(IDapperCommandFactory), commandFactoryType);
        }
    }
}
