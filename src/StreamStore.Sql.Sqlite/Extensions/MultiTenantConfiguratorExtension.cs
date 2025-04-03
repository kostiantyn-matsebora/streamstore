using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using System;

namespace StreamStore.Sql.Sqlite
{
    public static class MultiTenantConfiguratorExtension
    {
        public static IMultitenancyConfigurator UseSqliteStorage(
              this IMultitenancyConfigurator configurator,
              Action<SqlMultiTenantStorageConfigurator> dbConfigurator)
        {
            return configurator
                .UseSchemaProvisionerFactory<SqliteSchemaProvisionerFactory>()
                .UseSqlStorage<SqliteTenantStorageProvider>(SqliteConfiguration.DefaultConfiguration, (c) =>
                {
                    ConfigureRequiredDependencies(c);
                    dbConfigurator(c);
                });
        }

        public static IMultitenancyConfigurator UseSqliteStorage(
            this IMultitenancyConfigurator configurator, 
            IConfiguration configuration, 
            Action<SqlMultiTenantStorageConfigurator> dbConfigurator)
        {
            return configurator
                 .UseSchemaProvisionerFactory<SqliteSchemaProvisionerFactory>()
                 .UseSqlStorage<SqliteTenantStorageProvider>(SqliteConfiguration.DefaultConfiguration, configuration, SqliteConfiguration.ConfigurationSection, (c) =>
                 {
                     ConfigureRequiredDependencies(c);
                     dbConfigurator(c);
                 });
        }

        static void ConfigureRequiredDependencies(SqlMultiTenantStorageConfigurator configurator)
        {
            configurator.WithExceptionHandling<SqliteExceptionHandler>();
        }
    }
}
