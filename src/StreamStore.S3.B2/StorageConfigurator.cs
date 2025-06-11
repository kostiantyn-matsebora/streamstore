using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Storage.Configuration;

namespace StreamStore.S3.B2
{
    internal class StorageConfigurator : StorageConfiguratorBase
    {
        readonly B2StorageConfigurator configurator;

        public StorageConfigurator(B2StorageConfigurator configurator)
        {
            this.configurator = configurator.ThrowIfNull(nameof(configurator));
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
            services.CopyFrom(configurator.Configure());
        }
    }
}
