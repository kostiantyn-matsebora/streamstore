using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Storage.Configuration;

namespace StreamStore.InMemory.Configuration
{
    internal class StorageConfigurator : StorageConfiguratorBase
    {
        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            services.RegisterDomainValidation();
        }

        protected override void ConfigureSchemaProvisioner(SchemaProvisionerRegistrator registrator)
        {
            registrator.RegisterDummySchemaProvisioner();
        }

        protected override void ConfigureStorage(StorageDependencyRegistrator registrator)
        {
          registrator.RegisterStorage<InMemoryStreamStorage>();
        }
    }
}
