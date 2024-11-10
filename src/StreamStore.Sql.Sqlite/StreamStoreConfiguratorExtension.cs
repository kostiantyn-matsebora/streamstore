using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.Sqlite
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UseSqliteDatabase(this IStreamStoreConfigurator configurator, IConfiguration configuration)
        {
            return configurator.UseSqlDatabase(ConfigureDependencies, configuration, "StreamStore:Postgres");
        }

        public static IStreamStoreConfigurator UseSqliteDatabase(this IStreamStoreConfigurator configurator, Action<SqlDatabaseConfigurator> dbConfigurator)
        {
            return configurator.UseSqlDatabase(ConfigureDependencies, dbConfigurator);
        }

        static void ConfigureDependencies(SqlDatabaseDependencyConfigurator dependencyConfigurator)
        {
            dependencyConfigurator.WithConnectionFactory<SqliteDbConnectionFactory>();
            dependencyConfigurator.WithExceptionHandling<SqliteExceptionHandler>();
            dependencyConfigurator.WithProvisioingQueryProvider<SqliteProvisioningQueryProvider>();
        }
    }
}
