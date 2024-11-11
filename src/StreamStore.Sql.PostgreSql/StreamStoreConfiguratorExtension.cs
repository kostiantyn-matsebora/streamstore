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
            return configurator.UseSqlDatabase(DefaultConfiguration, ConfigureDependencies, configuration, configurationSection);
        }

        public static IStreamStoreConfigurator UsePostgresDatabase(this IStreamStoreConfigurator configurator, Action<SqlDatabaseConfigurator> dbConfigurator)
        {
            return configurator.UseSqlDatabase(DefaultConfiguration, ConfigureDependencies, dbConfigurator);
        }

        static void ConfigureDependencies(SqlDatabaseDependencyConfigurator dependencyConfigurator)
        {
            dependencyConfigurator.WithConnectionFactory<PostgresConnectionFactory>();
            dependencyConfigurator.WithExceptionHandling<PostgresExceptionHandler>();
            dependencyConfigurator.WithProvisioingQueryProvider<PostgresProvisioningQueryProvider>();
        }


        static SqlDatabaseConfiguration DefaultConfiguration = new SqlDatabaseConfiguration
        {
            SchemaName = "public",
            TableName = "Events",
            ProvisionSchema = true
        };
    }
}
