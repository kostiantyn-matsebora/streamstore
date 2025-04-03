using System;

using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.Sqlite
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantConfigurator UseSqliteStorage(this ISingleTenantConfigurator configurator, IConfiguration configuration)
        {
            return configurator.UseSqlStorage(SqliteConfiguration.DefaultConfiguration, configuration, SqliteConfiguration.ConfigurationSection, ConfigureRequiredDependencies);
        }

        public static ISingleTenantConfigurator UseSqliteStorage(
                this ISingleTenantConfigurator configurator, 
                Action<SqlSingleTenantStorageConfigurator> dbConfigurator)
        {
            return configurator.UseSqlStorage(SqliteConfiguration.DefaultConfiguration, (c) =>
            {
                ConfigureRequiredDependencies(c);
                dbConfigurator(c);
            });
        }

        static void ConfigureRequiredDependencies(SqlSingleTenantStorageConfigurator configurator)
        {
            configurator.WithConnectionFactory<SqliteDbConnectionFactory>();
            configurator.WithExceptionHandling<SqliteExceptionHandler>();
            configurator.WithProvisioingQueryProvider<SqliteProvisioningQueryProvider>();
        }
    }
}
