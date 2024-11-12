using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.API;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UseSqlDatabase(
                this IStreamStoreConfigurator configurator,
                SqlDatabaseConfiguration defaultConfig,
                Action<SqlDatabaseDependencyConfigurator> dependencyConfigurator,
                IConfiguration configuration,
                string sectionName)
        {
            configurator.WithDatabase(c =>
            {
                c.ConfigureWith(services =>
                {
                    // Configuring database
                    new SqlDatabaseConfigurator(services, defaultConfig).Configure(configuration, sectionName);

                    // Configuring dependencies
                    ConfigureDependencies(dependencyConfigurator, services);
                });
            });

            return configurator;
        }

        public static IStreamStoreConfigurator UseSqlDatabase(
                this IStreamStoreConfigurator configurator,
                SqlDatabaseConfiguration defaultConfig,
                Action<SqlDatabaseDependencyConfigurator> dependencyConfigurator,
                Action<SqlDatabaseConfigurator> dbConfigurator)
        {
            configurator.WithDatabase(c =>
            {
                c.ConfigureWith(services =>
                {
                    // Configuring database
                    var configurator = new SqlDatabaseConfigurator(services, defaultConfig);
                    dbConfigurator(configurator);
                    configurator.Configure();

                    // Configuring dependencies
                    ConfigureDependencies(dependencyConfigurator, services);
                });
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
