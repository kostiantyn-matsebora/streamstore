using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.SQL;
using StreamStore.SQL.Sqlite;

namespace StreamStore.Sql.Sqlite
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UseSqlDatabase(
                this IStreamStoreConfigurator configurator, 
                Action<SqlDatabaseDependencyConfigurator> dependencyConfigurator,
                IConfiguration configuration, 
                string sectionName = "StreamStore:Sql")
        {
            configurator.WithDatabase(services =>
            {
                // Configuring database
                new SqlDatabaseConfigurator(services).Configure(configuration, sectionName);

                // Configuring dependencies
                ConfigureDependencies(dependencyConfigurator, services);

            });

            return configurator;
        }

        public static IStreamStoreConfigurator UseSqlDatabase(
                this IStreamStoreConfigurator configurator, 
                Action<SqlDatabaseDependencyConfigurator> dependencyConfigurator, 
                Action<SqlDatabaseConfigurator> dbConfigurator)
        {
            configurator.WithDatabase(services =>
            {
                // Configuring database
                var configurator = new SqlDatabaseConfigurator(services);
                dbConfigurator(configurator);
                configurator.Configure();

                // Configuring dependencies
                ConfigureDependencies(dependencyConfigurator, services);
            });

            return configurator;
        }

        static void ConfigureDependencies(Action<SqlDatabaseDependencyConfigurator> dependencyConfigurator, IServiceCollection services)
        {
            var depConfigurator = new SqlDatabaseDependencyConfigurator();
            dependencyConfigurator(depConfigurator);
            depConfigurator.Configure(services);

            if (!services.Any(s => s.ServiceType == typeof(IDbConnectionFactory)))
            {
                throw new InvalidOperationException("Database connection factory (IDbConnectionFactory) is not registered");
            }

            if (!services.Any(s => s.ServiceType == typeof(IDapperCommandFactory)))
            {
                throw new InvalidOperationException("Dapper command factory (IDapperCommandFactory) is not registered");
            }
        }
    }
}
