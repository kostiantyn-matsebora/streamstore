using Microsoft.Extensions.Configuration;
using StreamStore.Multitenancy;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;
using System;

namespace StreamStore.Sql
{
    public static class MultiTenantConfiguratorExtension
    {
        internal static IMultitenancyConfigurator UseSqlStorage<TStorageProvider>(
                this IMultitenancyConfigurator configurator,
                SqlStorageConfiguration defaultConfig,
                IConfiguration configuration,
                string sectionName,
                Action<SqlMultiTenantStorageConfigurator> configureStorage) where TStorageProvider : ITenantStreamStorageProvider
        {
            return configurator.UseStorageProvider<TStorageProvider>(services =>
            {
                var configurator = new SqlMultiTenantStorageConfigurator(services, defaultConfig);
                configureStorage(configurator);
                configurator.ApplyFromConfig(configuration, sectionName);
            });
            
        }

        internal static IMultitenancyConfigurator UseSqlStorage<TStorageProvider>(
             this IMultitenancyConfigurator configurator,
             SqlStorageConfiguration defaultConfig,
             Action<SqlMultiTenantStorageConfigurator> configureStorage) where TStorageProvider: ITenantStreamStorageProvider
        {
            return configurator.UseStorageProvider<TStorageProvider>(services =>
            {
                var dbConfigurator = new SqlMultiTenantStorageConfigurator(services, defaultConfig);
                configureStorage(dbConfigurator);
                dbConfigurator.Apply();
            });
        }

    }
   
}
