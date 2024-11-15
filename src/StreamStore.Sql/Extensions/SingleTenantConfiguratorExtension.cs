using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Database;
using StreamStore.Sql.Provisioning;

namespace StreamStore.Sql
{
    public static class SingleTenantConfiguratorExtension
    {
        internal static ISingleTenantConfigurator UseSqlDatabase(
                this ISingleTenantConfigurator configurator,
                SqlDatabaseConfiguration defaultConfig,
                IConfiguration configuration,
                string sectionName,
                Action<SqlSingleTenantDatabaseConfigurator> configureDatabase)
        {
            return configurator
                .UseSchemaProvisioner<SqlSchemaProvisioner>()
                .UseDatabase<SqlStreamDatabase>(services =>
                {
                    // Configuring database
                    var dbConfigurator = new SqlSingleTenantDatabaseConfigurator(services, defaultConfig);
                    configureDatabase(dbConfigurator);
                    dbConfigurator.ApplyFromConfig(configuration, sectionName);
                });
        }

        internal static ISingleTenantConfigurator UseSqlDatabase(
                this ISingleTenantConfigurator configurator,
                SqlDatabaseConfiguration defaultConfig,
                Action<SqlSingleTenantDatabaseConfigurator> configureDatabase)
        {

            return configurator
                .UseSchemaProvisioner<SqlSchemaProvisioner>()
                .UseDatabase<SqlStreamDatabase>(services =>
                {
                    // Configuring database
                    var dbConfigurator = new SqlSingleTenantDatabaseConfigurator(services, defaultConfig);
                    configureDatabase(dbConfigurator);
                    dbConfigurator.Apply();
                });
        }

    }
}
