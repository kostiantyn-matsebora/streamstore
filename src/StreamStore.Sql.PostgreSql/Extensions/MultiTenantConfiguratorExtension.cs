using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using System;

namespace StreamStore.Sql.PostgreSql
{
    public static class MultiTenantConfiguratorExtension
    {
        internal static IMultitenancyConfigurator UsePostgresDatabase(
              this IMultitenancyConfigurator configurator,
              Action<SqlMultiTenantDatabaseConfigurator> dbConfigurator)
        {
            return configurator
                .UseDatabaseProvider<PostgresTenantDatabaseProvider>()
                .UseSchemaProvisionerFactory<PostgresSchemaProvisionerFactory>()
                .UseSqlDatabase<PostgresTenantDatabaseProvider>(PostgresConfiguration.DefaultConfiguration, (c) =>
                {
                    ConfigureRequiredDependencies(c);
                    dbConfigurator(c);
                });
        }

        public static IMultitenancyConfigurator UsePostgresDatabase(
            this IMultitenancyConfigurator configurator, 
            IConfiguration configuration,
            Action<SqlMultiTenantDatabaseConfigurator> dbConfigurator)
        {
            return configurator
                 .UseDatabaseProvider<PostgresTenantDatabaseProvider>()
                 .UseSchemaProvisionerFactory<PostgresSchemaProvisionerFactory>()
                 .UseSqlDatabase<PostgresTenantDatabaseProvider>(PostgresConfiguration.DefaultConfiguration, configuration, PostgresConfiguration.ConfigurationSection, (c) =>
                 {
                     ConfigureRequiredDependencies(c);
                     dbConfigurator(c);
                 });
        }

        static void ConfigureRequiredDependencies(SqlMultiTenantDatabaseConfigurator configurator)
        {
            configurator.WithExceptionHandling<PostgresExceptionHandler>();
        }
    }
}
