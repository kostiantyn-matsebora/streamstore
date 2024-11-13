using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using System;

namespace StreamStore.Sql.Sqlite
{
    public static class MultiTenantConfiguratorExtension
    {
        internal static IMultitenantDatabaseConfigurator UseSqliteDatabase(
              this IMultitenantDatabaseConfigurator configurator,
              Action<SqlMultiTenantDatabaseConfigurator> dbConfigurator)
        {
            return configurator
                .UseDatabaseProvider<SqliteTenantDatabaseProvider>()
                .UseSchemaProvisionerFactory<SqliteSchemaProvisionerFactory>()
                .UseSqlDatabase(Configuration.DefaultConfiguration, (c) =>
                {
                    ConfigureRequiredDependencies(c);
                    dbConfigurator(c);
                });
        }

        public static IMultitenantDatabaseConfigurator UseSqliteDatabase(this IMultitenantDatabaseConfigurator configurator, IConfiguration configuration)
        {
            return configurator
                 .UseDatabaseProvider<SqliteTenantDatabaseProvider>()
                 .UseSchemaProvisionerFactory<SqliteSchemaProvisionerFactory>()
                 .UseSqlDatabase(Configuration.DefaultConfiguration, configuration, Configuration.ConfigurationSection, ConfigureRequiredDependencies);
        }

        static void ConfigureRequiredDependencies(SqlMultiTenantDatabaseConfigurator configurator)
        {
            configurator.WithExceptionHandling<SqliteExceptionHandler>();
        }
    }
}
