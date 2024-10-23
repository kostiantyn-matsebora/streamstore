using Microsoft.Extensions.Configuration;
using StreamStore.SQL.Sqlite;

namespace StreamStore.Sql.Sqlite
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UseSqliteDatabase(this IStreamStoreConfigurator configurator, IConfiguration configuration)
        {
            configurator.WithDatabase(services => {
                var configurator = new SqliteDatabaseConfigurator(services);
                configurator.Configure(configuration);
            });

            return configurator;
        }
    }
}
