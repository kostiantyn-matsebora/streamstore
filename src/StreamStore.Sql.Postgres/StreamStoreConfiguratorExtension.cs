using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;


namespace StreamStore.Sql.Postgres
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UsePostgresDatabase(this IStreamStoreConfigurator configurator, IConfiguration configuration)
        {
            return configurator.UseSqlDatabase(ConfigureDependencies, configuration, "StreamStore:Sqlite");
        }

        public static IStreamStoreConfigurator UsePostgresDatabase(this IStreamStoreConfigurator configurator, Action<SqlDatabaseConfigurator> dbConfigurator)
        {
            return configurator.UseSqlDatabase(ConfigureDependencies, dbConfigurator);
        }

        static void ConfigureDependencies(SqlDatabaseDependencyConfigurator dependencyConfigurator)
        {
            dependencyConfigurator.WithConnectionFactory<PostgresConnectionFactory>();
            dependencyConfigurator.WithExceptionHandling<PostgresExceptionHandler>();
            dependencyConfigurator.WithProvisioingQueryProvider<PostgresProvisioningQueryProvider>();
        }
    }
}
