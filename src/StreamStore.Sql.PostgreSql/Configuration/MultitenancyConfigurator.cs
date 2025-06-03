using Microsoft.Extensions.DependencyInjection;
using StreamStore.Storage.Configuration;

namespace StreamStore.Sql.PostgreSql
{
    internal class MultitenancyConfigurator : MultitenancyConfiguratorBase
    {
        public MultitenancyConfigurator()
        {
        }

        protected override void ConfigureStorageProvider(StorageProviderRegistrator registrator)
        {
            registrator.RegisterStorageProvider(serviceProvider => serviceProvider.GetRequiredService<PostgresTenantStorageProvider>().GetStorage);
        }

        protected override void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator)
        {
            registrator.RegisterSchemaProvisioningFactory(provider =>
                provider.GetRequiredService<PostgresSchemaProvisionerFactory>().Create);
        }
    }
}