using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.PostgreSql
{
    public static class SingleTenantRegistratorExtension
    {
        const string configurationSection = "StreamStore:PostgreSql";

        public static IStreamDatabaseRegistrator UsePostgresDatabase(this ISingleTenantDatabaseRegistrator registrator, IConfiguration configuration)
        {
            return registrator.UseSqlDatabase(DefaultConfiguration, configuration, configurationSection, ConfigureRequiredDependencies);

        }

        public static IStreamDatabaseRegistrator UsePostgresDatabase(this ISingleTenantDatabaseRegistrator registrator, Action<SqlSingleTenantDatabaseConfigurator> dbConfigurator)
        {
            return registrator.UseSqlDatabase(DefaultConfiguration, (c) =>
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


        static SqlDatabaseConfiguration DefaultConfiguration = new SqlDatabaseConfiguration
        {
            SchemaName = "public",
            TableName = "Events",
        };
    }
}
