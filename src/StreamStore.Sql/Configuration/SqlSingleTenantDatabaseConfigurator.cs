using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using StreamStore.Sql.API;
using StreamStore.Sql.Provisioning;



namespace StreamStore.Sql.Configuration
{
    public sealed class SqlSingleTenantDatabaseConfigurator: SqlDatabaseConfiguratorBase<SqlSingleTenantDatabaseConfigurator>
    {
        Type? connectionFactoryType;

        public SqlSingleTenantDatabaseConfigurator(IServiceCollection services, SqlDatabaseConfiguration defaultConfig): base(services, defaultConfig)
        {
        }


        public SqlSingleTenantDatabaseConfigurator WithConnectionFactory<TFactory>() where TFactory : IDbConnectionFactory
        {
            connectionFactoryType = typeof(TFactory);
            return this;
        }

        protected override IServiceCollection ApplyModeSpecificDependencies(SqlDatabaseConfiguration configuration, IServiceCollection services)
        {
            if (connectionFactoryType == null)
                throw new InvalidOperationException("IDbConnectionFactory type not set");
            
            services.AddSingleton(typeof(IDbConnectionFactory), connectionFactoryType);
            services.AddSingleton<ISchemaProvisioner, SqlSchemaProvisioner>();
            return services;
        }
    }
}
