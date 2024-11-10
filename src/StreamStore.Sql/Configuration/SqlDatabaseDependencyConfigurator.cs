using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.API;
using StreamStore.Sql.Database;

namespace StreamStore.Sql.Configuration
{
    public class SqlDatabaseDependencyConfigurator
    {
        Type commandFactoryType = typeof(DefaultDapperCommandFactory);
        Type sqlExceptionHandlerType = typeof(DefaultSqlExceptionHandler);
        Type sqlQueryProviderType = typeof(DefaultSqlQueryProvider);

        Type? connectionFactoryType;
        Type? sqlProvisionQueryProviderType;

        public SqlDatabaseDependencyConfigurator WithCommandFactory<TFactory>() where TFactory : IDapperCommandFactory
        {
            commandFactoryType = typeof(TFactory);
            return this;
        }

        public SqlDatabaseDependencyConfigurator WithConnectionFactory<TFactory>() where TFactory : IDbConnectionFactory
        {
            connectionFactoryType = typeof(TFactory);
            return this;
        }

        public SqlDatabaseDependencyConfigurator WithExceptionHandling<THandler>() where THandler : ISqlExceptionHandler
        {
            sqlExceptionHandlerType = typeof(THandler);
            return this;
        }

        public SqlDatabaseDependencyConfigurator WithQueryProvider<TProvider>() where TProvider : ISqlQueryProvider
        {
            sqlQueryProviderType = typeof(TProvider);
            return this;
        }

        public SqlDatabaseDependencyConfigurator WithProvisioingQueryProvider<TProvisioningProvider>() where TProvisioningProvider : ISqlProvisionQueryProvider
        {
            sqlProvisionQueryProviderType = typeof(TProvisioningProvider);
            return this;
        }

        public void Configure(IServiceCollection services)
        {
            if (connectionFactoryType == null)
                throw new InvalidOperationException("IDbConnectionFactory type not set");
            if (sqlProvisionQueryProviderType == null)
                throw new InvalidOperationException("ISqlProvisionQueryProvider type not set");

            services.AddSingleton(typeof(IDapperCommandFactory), commandFactoryType);
            services.AddSingleton(typeof(IDbConnectionFactory), connectionFactoryType);
            services.AddSingleton(typeof(ISqlExceptionHandler), sqlExceptionHandlerType);
            services.AddSingleton(typeof(ISqlQueryProvider), sqlQueryProviderType);
            services.AddSingleton(typeof(ISqlProvisionQueryProvider), sqlProvisionQueryProviderType);
        }
    }
}
