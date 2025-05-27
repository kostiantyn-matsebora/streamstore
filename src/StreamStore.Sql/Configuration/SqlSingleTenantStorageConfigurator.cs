using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using StreamStore.Sql.API;
using StreamStore.Sql.Storage;
using StreamStore.Sql.Provisioning;



namespace StreamStore.Sql.Configuration
{
    public sealed class SqlSingleTenantStorageConfigurator : SqlStorageConfiguratorBase<SqlSingleTenantStorageConfigurator>
    {
        Type? connectionFactoryType;
        Type sqlQueryProviderType = typeof(DefaultSqlQueryProvider);
        Type commandFactoryType = typeof(DefaultDapperCommandFactory);
        Type? migratorType;

        public SqlSingleTenantStorageConfigurator(IServiceCollection services, SqlStorageConfiguration defaultConfig) : base(services, defaultConfig)
        {
        }


        public SqlSingleTenantStorageConfigurator WithConnectionFactory<TFactory>() where TFactory : IDbConnectionFactory
        {
            connectionFactoryType = typeof(TFactory);
            return this;
        }

        public SqlSingleTenantStorageConfigurator WithQueryProvider<TProvider>() where TProvider : ISqlQueryProvider
        {
            sqlQueryProviderType = typeof(TProvider);
            return this;
        }

        public SqlSingleTenantStorageConfigurator WithCommandFactory<TFactory>() where TFactory : IDapperCommandFactory
        {
            commandFactoryType = typeof(TFactory);
            return this;
        }

        public SqlSingleTenantStorageConfigurator WithMigrator<TMigrator>() where TMigrator : IMigrator
        {
            migratorType = typeof(TMigrator);
            return this;
        }



        protected override void ApplySpecificDependencies(SqlStorageConfiguration configuration, IServiceCollection services)
        {
            if (connectionFactoryType == null)
                throw new InvalidOperationException("IDbConnectionFactory type not set");

            if (migratorType == null)
                throw new InvalidOperationException("IMigrator type not set");

            services.AddSingleton(typeof(IDbConnectionFactory), connectionFactoryType);
            services.AddSingleton(typeof(ISqlQueryProvider), sqlQueryProviderType);
            services.AddSingleton<ISchemaProvisioner, SqlSchemaProvisioner>();
            services.AddSingleton(typeof(IDapperCommandFactory), commandFactoryType);
            services.AddSingleton(typeof(IMigrator), migratorType);
        }
    }
}
