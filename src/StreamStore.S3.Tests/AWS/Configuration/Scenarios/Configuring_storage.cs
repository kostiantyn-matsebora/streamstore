using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;
using StreamStore.S3.AWS;

namespace StreamStore.S3.Tests.AWS.Configuration
{
    public class Configuring_storage : StorageConfiguratorScenario
    {
        protected override IStorageConfigurator CreateConfigurator()
        {
            return new StorageConfigurator(new AWSS3StorageSettings());
        }
    }
}
