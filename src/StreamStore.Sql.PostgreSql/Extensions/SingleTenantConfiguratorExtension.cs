using System;

using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.PostgreSql.Provisioning;


namespace StreamStore.Sql.PostgreSql
{
    public static class SingleTenantConfiguratorExtension
    {
        public static ISingleTenantConfigurator UsePostgresStorage(this ISingleTenantConfigurator configurator, IConfiguration configuration)
        {
            return configurator.UseSqlStorage(PostgresConfiguration.DefaultConfiguration, configuration, PostgresConfiguration.ConfigurationSection, ConfigureRequiredDependencies);
        }

        public static ISingleTenantConfigurator UsePostgresStorage(
                this ISingleTenantConfigurator configurator, 
                Action<SqlSingleTenantStorageConfigurator> dbConfigurator)
        {
            return configurator.UseSqlStorage(PostgresConfiguration.DefaultConfiguration, (c) =>
            {
                ConfigureRequiredDependencies(c);
                dbConfigurator(c);
            });
        }

        static void ConfigureRequiredDependencies(SqlSingleTenantStorageConfigurator configurator)
        {
            configurator.WithConnectionFactory<PostgresConnectionFactory>();
            configurator.WithExceptionHandling<PostgresExceptionHandler>();
            configurator.WithMigrator<PostgreSqlMigrator>();
            configurator.WithMigrationAssembly(typeof(PostgreSqlMigrator).Assembly);
        }
    }
}
