using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Storage.Configuration;

namespace StreamStore.S3.AWS
{
    internal class StorageConfigurator : StorageConfiguratorBase
    {
        readonly AWSS3StorageSettings settings;

        public StorageConfigurator(AWSS3StorageSettings settings)
        {
           this.settings = settings.ThrowIfNull(nameof(settings));
        }

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
            new AWSS3StorageConfigurator(services, settings).Configure();
        }
    }
}
