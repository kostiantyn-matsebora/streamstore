using System;
using Microsoft.Extensions.Configuration;
using StreamStore.SQL.Sqlite;

namespace StreamStore.Sql.Sqlite
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UseSqliteDatabase(this IStreamStoreConfigurator configurator, IConfiguration configuration)
        {
            configurator.WithDatabase(services => 
            {
                new SqliteDatabaseConfigurator(services).Configure(configuration);
            });

            return configurator;
        }

        public static IStreamStoreConfigurator UseSqliteDatabase(this IStreamStoreConfigurator configurator, Action<SqliteDatabaseConfigurator> dbConfigurator)
        {
            configurator.WithDatabase(services =>
            {
                var configurator = new SqliteDatabaseConfigurator(services);
                dbConfigurator(configurator);
                configurator.Configure();
            });

            return configurator;
        }
    }
}
