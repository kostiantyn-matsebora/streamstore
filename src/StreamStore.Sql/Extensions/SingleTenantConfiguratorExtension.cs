using System;
using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Storage;
using StreamStore.Sql.Provisioning;

namespace StreamStore.Sql
{
    public static class SingleTenantConfiguratorExtension
    {
        internal static ISingleTenantConfigurator UseSqlStorage(
                this ISingleTenantConfigurator configurator,
                SqlStorageConfiguration defaultConfig,
                IConfiguration configuration,
                string sectionName,
                Action<SqlSingleTenantStorageConfigurator> configureStorage)
        {
            return configurator
                .UseSchemaProvisioner<SqlSchemaProvisioner>()
                .UseStorage<SqlStreamStorage>(services =>
                {
                    // Configuring storage
                    var dbConfigurator = new SqlSingleTenantStorageConfigurator(services, defaultConfig);
                    configureStorage(dbConfigurator);
                    dbConfigurator.ApplyFromConfig(configuration, sectionName);
                });
        }

        internal static ISingleTenantConfigurator UseSqlStorage(
                this ISingleTenantConfigurator configurator,
                SqlStorageConfiguration defaultConfig,
                Action<SqlSingleTenantStorageConfigurator> configureStorage)
        {

            return configurator
                .UseSchemaProvisioner<SqlSchemaProvisioner>()
                .UseStorage<SqlStreamStorage>(services =>
                {
                    // Configuring storage
                    var dbConfigurator = new SqlSingleTenantStorageConfigurator(services, defaultConfig);
                    configureStorage(dbConfigurator);
                    dbConfigurator.Apply();
                });
        }

    }
}
