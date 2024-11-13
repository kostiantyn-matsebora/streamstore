using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Database;
using StreamStore.Sql.Provisioning;

namespace StreamStore.Sql
{
    public static class SingleTenantConfiguratorExtension
    {
        internal static ISingleTenantDatabaseConfigurator UseSqlDatabase(
                this ISingleTenantDatabaseConfigurator registrator,
                SqlDatabaseConfiguration defaultConfig,
                IConfiguration configuration,
                string sectionName,
                Action<SqlSingleTenantDatabaseConfigurator> configurator)
        {


            return registrator
                .UseSchemaProvisioner<SqlSchemaProvisioner>()
                .UseDatabase<SqlStreamDatabase>(services =>
            {
                // Configuring database
                new SqlSingleTenantDatabaseConfigurator(services, defaultConfig)
                .ApplyFromConfig(configuration, sectionName);
            });
        }

        internal static ISingleTenantDatabaseConfigurator UseSqlDatabase(
                this ISingleTenantDatabaseConfigurator registrator,
                SqlDatabaseConfiguration defaultConfig,
                Action<SqlSingleTenantDatabaseConfigurator> configurator)
        {

            return registrator
                .UseSchemaProvisioner<SqlSchemaProvisioner>()
                .UseDatabase<SqlStreamDatabase>(services =>
                {
                    // Configuring database
                    var dbConfigurator = new SqlSingleTenantDatabaseConfigurator(services, defaultConfig);
                    configurator(dbConfigurator);
                    dbConfigurator.Apply();
                });
        }
    }
}
