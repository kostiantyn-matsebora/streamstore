using StreamStore.Storage.Configuration;

namespace StreamStore.InMemory.Configuration
{
    internal class StorageConfigurator : StorageConfiguratorBase
    {
        protected override void ConfigureSchemaProvisioner(SchemaProvisionerRegistrator registrator)
        {
            registrator.RegisterDummSchemaProvisioner();
        }

        protected override void ConfigureStorage(StorageDependencyRegistrator registrator)
        {
          registrator.RegisterStorage<InMemoryStreamStorage>();
        }
    }
}
