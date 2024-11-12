using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;

namespace StreamStore.Sql
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamDatabaseRegistrator UseSqlDatabase(
                this IStreamDatabaseRegistrator registrator,
                SqlDatabaseConfiguration defaultConfig,
                IConfiguration configuration,
                string sectionName)
        {

            registrator.ConfigureWith(services =>
            {
                // Configuring database
                new SqlDatabaseConfigurator(services, defaultConfig)
                    .Configure(configuration, sectionName, registrator.MultiTenancyEnabled);
            });

            return registrator;
        }

        public static IStreamDatabaseRegistrator UseSqlDatabase(
                this IStreamDatabaseRegistrator registrator,
                SqlDatabaseConfiguration defaultConfig,
                Action<SqlDatabaseConfigurator> configurator)
        {

            registrator.ConfigureWith(services =>
                {
                    // Configuring database
                    var dbConfigurator = new SqlDatabaseConfigurator(services, defaultConfig);
                    configurator(dbConfigurator);
                    dbConfigurator.Configure(registrator.MultiTenancyEnabled);
                });
            return registrator;
        }

     
    }
}
