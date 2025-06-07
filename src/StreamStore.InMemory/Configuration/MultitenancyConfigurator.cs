using Microsoft.Extensions.DependencyInjection;
using StreamStore.Storage.Configuration;

namespace StreamStore.InMemory.Configuration
{
    internal class MultitenancyConfigurator : MultitenancyConfiguratorBase
    {
        protected override void ConfigureStorageProvider(StorageProviderRegistrator registrator)
        {
            registrator.RegisterStorageProvider(serviceProvider => serviceProvider.GetRequiredService<InMemoryStreamStorageProvider>().GetStorage);
        }

        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            services.AddSingleton<InMemoryStreamStorageProvider>();
        }

        protected override void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator)
        {
            registrator.RegisterDummySchemaProvisioningFactory();
        }
    }
}
