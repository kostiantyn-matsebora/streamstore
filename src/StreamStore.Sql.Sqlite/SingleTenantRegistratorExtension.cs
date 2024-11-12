using System;

using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.Sqlite
{
    public static class SingleTenantRegistratorExtension
    {
        const string configurationSection = "StreamStore:Sqlite";

        public static IStreamDatabaseRegistrator UseSqliteDatabase(this ISingleTenantDatabaseRegistrator registrator, IConfiguration configuration)
        {
            return registrator.UseSqlDatabase(DefaultConfiguration, configuration, configurationSection, ConfigureRequiredDependencies);

        }

        public static IStreamDatabaseRegistrator UseSqliteDatabase(
            this ISingleTenantDatabaseRegistrator registrator, 
            Action<SqlSingleTenantDatabaseConfigurator> dbConfigurator)
        {
            return registrator.UseSqlDatabase(DefaultConfiguration, (c) =>
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

        static SqlDatabaseConfiguration DefaultConfiguration = new SqlDatabaseConfiguration
        {
            SchemaName = "main",
            TableName = "Events",
        };
    }
}
