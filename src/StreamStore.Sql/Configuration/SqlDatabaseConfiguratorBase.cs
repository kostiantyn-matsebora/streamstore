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


        Type sqlExceptionHandlerType = typeof(DefaultSqlExceptionHandler);

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
            return Apply(configurationBuilder.Build());
        }

        IServiceCollection Apply(SqlDatabaseConfiguration configuration)
        {
            ApplySharedDependencies(configuration, services);
            ApplySpecificDependencies(configuration, services);

            return services;
        }

        protected abstract void ApplySpecificDependencies(SqlDatabaseConfiguration configuration, IServiceCollection services);

        void ApplySharedDependencies(SqlDatabaseConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton(configuration);
            services.AddSingleton(typeof(ISqlExceptionHandler), sqlExceptionHandlerType);
        }
    }
}
