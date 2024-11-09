using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.API;
using StreamStore.Sql.Database;

namespace StreamStore.Sql.Configuration
{
    public class SqlDatabaseDependencyConfigurator
    {
        Type? commandFactoryType;
        Type? connectionFactoryType;
        Type sqlExceptionHandlerType = typeof(DefaultSqlExceptionHandler);

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

        public void Configure(IServiceCollection services)
        {
            if (commandFactoryType == null)
                throw new InvalidOperationException("IDapperCommandFactory type not set");
            if (connectionFactoryType == null)
                throw new InvalidOperationException("IDbConnectionFactory type not set");

            services.AddSingleton(typeof(IDapperCommandFactory), commandFactoryType);
            services.AddSingleton(typeof(IDbConnectionFactory), connectionFactoryType);
            services.AddSingleton(typeof(ISqlExceptionHandler), sqlExceptionHandlerType);
        }
    }
}
