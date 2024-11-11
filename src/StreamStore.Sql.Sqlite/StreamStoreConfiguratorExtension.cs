using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.Sqlite
{
    public static class StreamStoreConfiguratorExtension
    {
        const string configurationSection = "StreamStore:Sqlite";

        public static IStreamStoreConfigurator UseSqliteDatabase(this IStreamStoreConfigurator configurator, IConfiguration configuration)
        {
            return configurator.UseSqlDatabase(DefaultConfiguration, ConfigureDependencies, configuration, configurationSection);
        }

        public static IStreamStoreConfigurator UseSqliteDatabase(this IStreamStoreConfigurator configurator, Action<SqlDatabaseConfigurator> dbConfigurator)
        {
            return configurator.UseSqlDatabase(DefaultConfiguration, ConfigureDependencies, dbConfigurator);
        }


        static void ConfigureDependencies(SqlDatabaseDependencyConfigurator dependencyConfigurator)
        {
            dependencyConfigurator.WithConnectionFactory<SqliteDbConnectionFactory>();
            dependencyConfigurator.WithExceptionHandling<SqliteExceptionHandler>();
            dependencyConfigurator.WithProvisioingQueryProvider<SqliteProvisioningQueryProvider>();
        }

        static SqlDatabaseConfiguration DefaultConfiguration = new SqlDatabaseConfiguration
        {
            SchemaName = "main",
            TableName = "Events",
            ProvisionSchema = true
        };
    }
}
