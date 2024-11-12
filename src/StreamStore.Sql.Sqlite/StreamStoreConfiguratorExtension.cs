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
           return configurator.WithDatabase(registrator =>
            {
                registrator.UseSqlDatabase(DefaultConfiguration, configuration, configurationSection);
            });
        }

        public static IStreamStoreConfigurator UseSqliteDatabase(this IStreamStoreConfigurator configurator, Action<SqlDatabaseConfigurator> dbConfigurator)
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
            configurator.WithConnectionFactory<SqliteDbConnectionFactory>();
            configurator.WithExceptionHandling<SqliteExceptionHandler>();
            configurator.WithProvisioingQueryProvider<SqliteProvisioningQueryProvider>();
        }

        static SqlDatabaseConfiguration DefaultConfiguration = new SqlDatabaseConfiguration
        {
            SchemaName = "main",
            TableName = "Events",
            ProvisionSchema = true
        };
    }
}
