using System;

using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.PostgreSql
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantDatabaseConfigurator UsePostgresDatabase(this ISingleTenantDatabaseConfigurator configurator, IConfiguration configuration)
        {
            return configurator.UseSqlDatabase(Configuration.DefaultConfiguration, configuration, Configuration.ConfigurationSection, ConfigureRequiredDependencies);
        }

        public static ISingleTenantDatabaseConfigurator UsePostgresDatabase(
                this ISingleTenantDatabaseConfigurator registrator, 
                Action<SqlSingleTenantDatabaseConfigurator> dbConfigurator)
        {
            return registrator.UseSqlDatabase(Configuration.DefaultConfiguration, (c) =>
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
