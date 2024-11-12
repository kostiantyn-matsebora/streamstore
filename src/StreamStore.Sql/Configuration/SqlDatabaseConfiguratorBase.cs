using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.API;
using StreamStore.Sql.Database;

namespace StreamStore.Sql.Configuration
{
    public abstract class SqlDatabaseConfiguratorBase<TConfigurator> where TConfigurator : SqlDatabaseConfiguratorBase<TConfigurator>
    {
        readonly SqlDatabaseConfigurationBuilder configurationBuilder;

        Type commandFactoryType = typeof(DefaultDapperCommandFactory);
        Type sqlExceptionHandlerType = typeof(DefaultSqlExceptionHandler);
        Type sqlQueryProviderType = typeof(DefaultSqlQueryProvider);

        Type? sqlProvisionQueryProviderType;

        readonly IServiceCollection services;

        protected SqlDatabaseConfiguratorBase(IServiceCollection services, SqlDatabaseConfiguration defaultConfig)
        {
            this.services = services.ThrowIfNull(nameof(services));
            configurationBuilder = new SqlDatabaseConfigurationBuilder(defaultConfig);
        }


        public TConfigurator WithExceptionHandling<THandler>() where THandler : ISqlExceptionHandler
        {
            sqlExceptionHandlerType = typeof(THandler);
            return (TConfigurator)this;
        }

        public TConfigurator WithQueryProvider<TProvider>() where TProvider : ISqlQueryProvider
        {
            sqlQueryProviderType = typeof(TProvider);
            return (TConfigurator)this;
        }

        public TConfigurator WithProvisioingQueryProvider<TProvisioningProvider>() where TProvisioningProvider : ISqlProvisioningQueryProvider
        {
            sqlProvisionQueryProviderType = typeof(TProvisioningProvider);
            return (TConfigurator)this;
        }

        public TConfigurator WithCommandFactory<TFactory>() where TFactory : IDapperCommandFactory
        {
            commandFactoryType = typeof(TFactory);
            return (TConfigurator)this;
        }

        public TConfigurator ConfigureDatabase(Action<SqlDatabaseConfigurationBuilder> configurator)
        {
            configurator(configurationBuilder);
            return (TConfigurator)this;
        }

        public IServiceCollection ApplyFromConfig(IConfiguration configuration, string sectionName)
        {
            return Apply(configurationBuilder.ReadFromConfig(configuration, sectionName));
        }

        public IServiceCollection Apply()
        {
            var configuration = configurationBuilder.Build();
            return Apply(configurationBuilder.Build());
        }

        IServiceCollection Apply(SqlDatabaseConfiguration configuration)
        {
            ApplySharedDependencies(configuration, services);
            ApplyModeSpecificDependencies(configuration, services);

            return services;
        }

        protected abstract IServiceCollection ApplyModeSpecificDependencies(SqlDatabaseConfiguration configuration, IServiceCollection services);

        IServiceCollection ApplySharedDependencies(SqlDatabaseConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton(configuration);

            if (sqlProvisionQueryProviderType == null)
                throw new InvalidOperationException("ISqlProvisionQueryProvider type not set");

            services.AddSingleton(typeof(IDapperCommandFactory), commandFactoryType);
            services.AddSingleton(typeof(ISqlExceptionHandler), sqlExceptionHandlerType);
            services.AddSingleton(typeof(ISqlQueryProvider), sqlQueryProviderType);
            services.AddSingleton(typeof(ISqlProvisioningQueryProvider), sqlProvisionQueryProviderType);

            return services;
        }
    }
}
