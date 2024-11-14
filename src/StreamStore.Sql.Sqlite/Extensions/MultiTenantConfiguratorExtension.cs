using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using System;

namespace StreamStore.Sql.Sqlite
{
    public static class MultiTenantConfiguratorExtension
    {
        public static IMultitenancyConfigurator UseSqliteDatabase(
              this IMultitenancyConfigurator configurator,
              Action<SqlMultiTenantDatabaseConfigurator> dbConfigurator)
        {
            return configurator
                .UseSchemaProvisionerFactory<SqliteSchemaProvisionerFactory>()
                .UseSqlDatabase<SqliteTenantDatabaseProvider>(SqliteConfiguration.DefaultConfiguration, (c) =>
                {
                    ConfigureRequiredDependencies(c);
                    dbConfigurator(c);
                });
        }

        public static IMultitenancyConfigurator UseSqliteDatabase(
            this IMultitenancyConfigurator configurator, 
            IConfiguration configuration, 
            Action<SqlMultiTenantDatabaseConfigurator> dbConfigurator)
        {
            return configurator
                 .UseSchemaProvisionerFactory<SqliteSchemaProvisionerFactory>()
                 .UseSqlDatabase<SqliteTenantDatabaseProvider>(SqliteConfiguration.DefaultConfiguration, configuration, SqliteConfiguration.ConfigurationSection, (c) =>
                 {
                     ConfigureRequiredDependencies(c);
                     dbConfigurator(c);
                 });
        }

        static void ConfigureRequiredDependencies(SqlMultiTenantDatabaseConfigurator configurator)
        {
            configurator.WithExceptionHandling<SqliteExceptionHandler>();
        }
    }
}
