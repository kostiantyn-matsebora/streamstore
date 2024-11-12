using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using StreamStore.Sql.API;
using StreamStore.Sql.Database;
using StreamStore.Sql.Provisioning;


namespace StreamStore.Sql.Configuration
{
    public sealed class SqlDatabaseConfigurator
    {
        readonly IServiceCollection services;
        readonly SqlDatabaseConfigurationBuilder configurationBuilder;


        Type commandFactoryType = typeof(DefaultDapperCommandFactory);
        Type sqlExceptionHandlerType = typeof(DefaultSqlExceptionHandler);
        Type sqlQueryProviderType = typeof(DefaultSqlQueryProvider);

        Type? connectionFactoryType;
        Type? sqlProvisionQueryProviderType;


        public SqlDatabaseConfigurator WithCommandFactory<TFactory>() where TFactory : IDapperCommandFactory
        {
            commandFactoryType = typeof(TFactory);
            return this;
        }

        public SqlDatabaseConfigurator WithConnectionFactory<TFactory>() where TFactory : IDbConnectionFactory
        {
            connectionFactoryType = typeof(TFactory);
            return this;
        }

        public SqlDatabaseConfigurator WithExceptionHandling<THandler>() where THandler : ISqlExceptionHandler
        {
            sqlExceptionHandlerType = typeof(THandler);
            return this;
        }

        public SqlDatabaseConfigurator WithQueryProvider<TProvider>() where TProvider : ISqlQueryProvider
        {
            sqlQueryProviderType = typeof(TProvider);
            return this;
        }

        public SqlDatabaseConfigurator WithProvisioingQueryProvider<TProvisioningProvider>() where TProvisioningProvider : ISqlProvisioningQueryProvider
        {
            sqlProvisionQueryProviderType = typeof(TProvisioningProvider);
            return this;
        }

        public SqlDatabaseConfigurator(IServiceCollection services, SqlDatabaseConfiguration defaultConfig)
        {
            this.services = services ?? throw new ArgumentNullException(nameof(services));
            configurationBuilder = new SqlDatabaseConfigurationBuilder(defaultConfig);
        }

        public IServiceCollection Configure(bool multitenancyEnabled)
        {
            return RegisterDependencies(configurationBuilder.Build(multitenancyEnabled));
        }

        public SqlDatabaseConfigurator ConfigureDatabase(Action<SqlDatabaseConfigurationBuilder> configurator)
        {
            configurator(configurationBuilder);
            return this;
        }

        public IServiceCollection Configure(IConfiguration configuration, string sectionName, bool multitenancyEnabled)
        {
            return RegisterDependencies(configurationBuilder.ReadFromConfig(configuration, sectionName, multitenancyEnabled));
        }

        IServiceCollection RegisterDependencies(SqlDatabaseConfiguration configuration)
        {
            return RegisterStreamStoreDependencies(
                        RegisterSqlSpecificDependencies(configuration, services));
        }

        IServiceCollection RegisterSqlSpecificDependencies(SqlDatabaseConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton(configuration);

            if (connectionFactoryType == null)
                throw new InvalidOperationException("IDbConnectionFactory type not set");
            if (sqlProvisionQueryProviderType == null)
                throw new InvalidOperationException("ISqlProvisionQueryProvider type not set");

            services.AddSingleton(typeof(IDapperCommandFactory), commandFactoryType);
            services.AddSingleton(typeof(IDbConnectionFactory), connectionFactoryType);
            services.AddSingleton(typeof(ISqlExceptionHandler), sqlExceptionHandlerType);
            services.AddSingleton(typeof(ISqlQueryProvider), sqlQueryProviderType);
            services.AddSingleton(typeof(ISqlProvisioningQueryProvider), sqlProvisionQueryProviderType);

            return services;
        }

        IServiceCollection RegisterStreamStoreDependencies(IServiceCollection services)
        {
            services.AddSingleton<IStreamDatabase, SqlStreamDatabase>();
            services.AddSingleton<IStreamReader, SqlStreamDatabase>();
            return services;
        }
    }
}
