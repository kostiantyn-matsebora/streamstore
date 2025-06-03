using Microsoft.Extensions.DependencyInjection;
using StreamStore.Storage.Configuration;

namespace StreamStore.Sql.Sqlite
{
    internal class MultitenancyConfigurator : MultitenancyConfiguratorBase
    {
        public MultitenancyConfigurator()
        {
        }

        protected override void ConfigureStorageProvider(StorageProviderRegistrator registrator)
        {
            registrator.RegisterStorageProvider(serviceProvider => serviceProvider.GetRequiredService<SqliteTenantStorageProvider>().GetStorage);
        }

        protected override void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator)
        {
            registrator.RegisterSchemaProvisioningFactory(provider =>
                provider.GetRequiredService<SqliteSchemaProvisionerFactory>().Create);
        }
    }
}