using Microsoft.Extensions.DependencyInjection;
using StreamStore.Storage.Configuration;

namespace StreamStore.S3.B2
{
    internal class StorageConfigurator : StorageConfiguratorBase
    {
        protected override void ConfigureSchemaProvisioner(SchemaProvisionerRegistrator registrator)
        {
            registrator.RegisterDummySchemaProvisioner();
        }

        protected override void ConfigureStorage(StorageDependencyRegistrator registrator)
        {
            registrator.RegisterStorage<S3StreamStorage>();
        }

        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            new B2StorageConfigurator(services).Configure();
        }
    }
}
