using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.PostgreSql.Provisioning;

namespace StreamStore.Sql.PostgreSql
{
    public static class MultiTenantConfiguratorExtension
    {
        public static IMultitenancyConfigurator UsePostgresStorage(
              this IMultitenancyConfigurator configurator,
              Action<SqlMultiTenantStorageConfigurator> dbConfigurator)
        {
            return configurator
                .UseStorageProvider<PostgresTenantStorageProvider>()
                .UseSchemaProvisionerFactory<PostgresSchemaProvisionerFactory>()
                .UseSqlStorage<PostgresTenantStorageProvider>(PostgresConfiguration.DefaultConfiguration, (c) =>
                {
                    ConfigureRequiredDependencies(c);
                    dbConfigurator(c);
                });
        }

        public static IMultitenancyConfigurator UsePostgresStorage(
            this IMultitenancyConfigurator configurator, 
            IConfiguration configuration,
            Action<SqlMultiTenantStorageConfigurator> dbConfigurator)
        {
            return configurator
                 .UseStorageProvider<PostgresTenantStorageProvider>()
                 .UseSchemaProvisionerFactory<PostgresSchemaProvisionerFactory>()
                 .UseSqlStorage<PostgresTenantStorageProvider>(PostgresConfiguration.DefaultConfiguration, configuration, PostgresConfiguration.ConfigurationSection, (c) =>
                 {
                     ConfigureRequiredDependencies(c);
                     dbConfigurator(c);
                 });
        }

        static void ConfigureRequiredDependencies(SqlMultiTenantStorageConfigurator configurator)
        {
            configurator.WithExceptionHandling<PostgresExceptionHandler>();
            configurator.WithMigrationAssembly(typeof(PostgreSqlMigrator).Assembly);
        }
    }
}
