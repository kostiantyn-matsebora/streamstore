using Microsoft.Extensions.Configuration;
using StreamStore.Multitenancy;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;
using System;

namespace StreamStore.Sql
{
    public static class MultiTenantConfiguratorExtension
    {
        internal static IMultitenantDatabaseConfigurator UseSqlDatabase<TDatabaseProvider>(
                this IMultitenantDatabaseConfigurator configurator,
                SqlDatabaseConfiguration defaultConfig,
                IConfiguration configuration,
                string sectionName,
                Action<SqlMultiTenantDatabaseConfigurator> configureDatabase) where TDatabaseProvider : ITenantStreamDatabaseProvider
        {
            return configurator.UseDatabaseProvider<TDatabaseProvider>(services =>
            {
                var configurator = new SqlMultiTenantDatabaseConfigurator(services, defaultConfig);
                configureDatabase(configurator);
                configurator.ApplyFromConfig(configuration, sectionName);
            });
            
        }

        internal static IMultitenantDatabaseConfigurator UseSqlDatabase<TDatabaseProvider>(
             this IMultitenantDatabaseConfigurator configurator,
             SqlDatabaseConfiguration defaultConfig,
             Action<SqlMultiTenantDatabaseConfigurator> configureDatabase) where TDatabaseProvider: ITenantStreamDatabaseProvider
        {
            return configurator.UseDatabaseProvider<TDatabaseProvider>(services =>
            {
                var dbConfigurator = new SqlMultiTenantDatabaseConfigurator(services, defaultConfig);
                configureDatabase(dbConfigurator);
                dbConfigurator.Apply();
            });
        }

    }
   
}
