using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql.PostgreSql
{
    public static class StreamStoreConfiguratorExtension
    {
        const string configurationSection = "StreamStore:PostgreSql";

        public static IStreamStoreConfigurator UsePostgresDatabase(this IStreamStoreConfigurator configurator, IConfiguration configuration)
        {
            return configurator.WithDatabase(registrator =>
            {

                registrator.UseSqlDatabase(DefaultConfiguration, configuration, configurationSection);
            });


        }

        public static IStreamStoreConfigurator UsePostgresDatabase(this IStreamStoreConfigurator configurator, Action<SqlDatabaseConfigurator> dbConfigurator)
        {
            return configurator.WithDatabase(registrator =>
            {
                registrator.UseSqlDatabase(DefaultConfiguration, c =>
                {
                    ConfigureDependencies(c);
                    dbConfigurator(c);
                });
            });
        }

        static void ConfigureDependencies(SqlDatabaseConfigurator configurator)
        {
            configurator.WithConnectionFactory<PostgresConnectionFactory>();
            configurator.WithExceptionHandling<PostgresExceptionHandler>();
            configurator.WithProvisioingQueryProvider<PostgresProvisioningQueryProvider>();
        }


        static SqlDatabaseConfiguration DefaultConfiguration = new SqlDatabaseConfiguration
        {
            SchemaName = "public",
            TableName = "Events",
            ProvisionSchema = true
        };
    }
}
