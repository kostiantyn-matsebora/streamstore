using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Database;

namespace StreamStore.Sql
{
    public static class SingleTenantRegistratorExtension
    {
        internal static IStreamDatabaseRegistrator UseSqlDatabase(
                this ISingleTenantDatabaseRegistrator registrator,
                SqlDatabaseConfiguration defaultConfig,
                IConfiguration configuration,
                string sectionName,
                Action<SqlSingleTenantDatabaseConfigurator> configurator)
        {

            registrator.RegisterDatabase<SqlStreamDatabase>();

            registrator.RegisterDependencies(services =>
            {
                // Configuring database
                new SqlSingleTenantDatabaseConfigurator(services, defaultConfig)
                .ApplyFromConfig(configuration, sectionName);
            });

            return registrator;
        }

        internal static IStreamDatabaseRegistrator UseSqlDatabase(
                this ISingleTenantDatabaseRegistrator registrator,
                SqlDatabaseConfiguration defaultConfig,
                Action<SqlSingleTenantDatabaseConfigurator> configurator)
        {

            registrator.RegisterDependencies(services =>
                {
                    // Configuring database
                    var dbConfigurator = new SqlSingleTenantDatabaseConfigurator(services, defaultConfig);
                    configurator(dbConfigurator);
                    dbConfigurator.Apply();
                });
            return registrator;
        }
    }
}
