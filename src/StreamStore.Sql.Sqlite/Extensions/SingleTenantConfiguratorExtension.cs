using System;

using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.Sqlite
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantConfigurator UseSqliteDatabase(this ISingleTenantConfigurator configurator, IConfiguration configuration)
        {
            return configurator.UseSqlDatabase(SqliteConfiguration.DefaultConfiguration, configuration, SqliteConfiguration.ConfigurationSection, ConfigureRequiredDependencies);
        }

        public static ISingleTenantConfigurator UseSqliteDatabase(
                this ISingleTenantConfigurator configurator, 
                Action<SqlSingleTenantDatabaseConfigurator> dbConfigurator)
        {
            return configurator.UseSqlDatabase(SqliteConfiguration.DefaultConfiguration, (c) =>
            {
                ConfigureRequiredDependencies(c);
                dbConfigurator(c);
            });
        }

        static void ConfigureRequiredDependencies(SqlSingleTenantDatabaseConfigurator configurator)
        {
            configurator.WithConnectionFactory<SqliteDbConnectionFactory>();
            configurator.WithExceptionHandling<SqliteExceptionHandler>();
            configurator.WithProvisioingQueryProvider<SqliteProvisioningQueryProvider>();
        }
    }
}
