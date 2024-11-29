using System;

using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.PostgreSql
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantConfigurator UsePostgresDatabase(this ISingleTenantConfigurator configurator, IConfiguration configuration)
        {
            return configurator.UseSqlDatabase(PostgresConfiguration.DefaultConfiguration, configuration, PostgresConfiguration.ConfigurationSection, ConfigureRequiredDependencies);
        }

        public static ISingleTenantConfigurator UsePostgresDatabase(
                this ISingleTenantConfigurator configurator, 
                Action<SqlSingleTenantDatabaseConfigurator> dbConfigurator)
        {
            return configurator.UseSqlDatabase(PostgresConfiguration.DefaultConfiguration, (c) =>
            {
                ConfigureRequiredDependencies(c);
                dbConfigurator(c);
            });
        }

        static void ConfigureRequiredDependencies(SqlSingleTenantDatabaseConfigurator configurator)
        {
            configurator.WithConnectionFactory<PostgresConnectionFactory>();
            configurator.WithExceptionHandling<PostgresExceptionHandler>();
            configurator.WithProvisioingQueryProvider<PostgresProvisioningQueryProvider>();
        }
    }
}
