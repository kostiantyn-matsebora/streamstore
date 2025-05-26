using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.API;
using StreamStore.Sql.Migrations;
using StreamStore.Sql.Storage;

namespace StreamStore.Sql.Configuration
{
    public abstract class SqlStorageConfiguratorBase<TConfigurator> where TConfigurator : SqlStorageConfiguratorBase<TConfigurator>
    {
        readonly SqlStorageConfigurationBuilder configurationBuilder;

        Type sqlExceptionHandlerType = typeof(DefaultSqlExceptionHandler);
        Assembly migrationAssembly = typeof(Initial).Assembly;

        readonly IServiceCollection services;

        protected SqlStorageConfiguratorBase(IServiceCollection services, SqlStorageConfiguration defaultConfig)
        {
            this.services = services.ThrowIfNull(nameof(services));
            configurationBuilder = new SqlStorageConfigurationBuilder(defaultConfig);
        }


        public TConfigurator WithExceptionHandling<THandler>() where THandler : ISqlExceptionHandler
        {
            sqlExceptionHandlerType = typeof(THandler);
            return (TConfigurator)this;
        }

        public TConfigurator WithMigrationAssenbly(Assembly assembly)
        {
            migrationAssembly = assembly;
            return (TConfigurator)this;
        }

        public TConfigurator ConfigureStorage(Action<SqlStorageConfigurationBuilder> configurator)
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

        IServiceCollection Apply(SqlStorageConfiguration configuration)
        {
            ApplySharedDependencies(configuration, services);
            ApplySpecificDependencies(configuration, services);

            return services;
        }

        protected abstract void ApplySpecificDependencies(SqlStorageConfiguration configuration, IServiceCollection services);

        void ApplySharedDependencies(SqlStorageConfiguration configuration, IServiceCollection services)
        {
            services.AddSingleton(configuration);
            services.AddSingleton(typeof(ISqlExceptionHandler), sqlExceptionHandlerType);
            services.AddSingleton(new MigrationConfiguration { MigrationAssembly = migrationAssembly });
        }
    }
}
