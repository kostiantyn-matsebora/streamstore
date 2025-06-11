using StreamStore.S3.B2;
using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2.Configuration
{
    public class Configuring_storage : StorageConfiguratorScenario
    {
        protected override IStorageConfigurator CreateConfigurator()
        {
            var configurator = new B2StorageConfigurator();
            configurator
                .WithCredential(Generated.Primitives.String, Generated.Primitives.String)
                .WithBucketId(Generated.Primitives.String);
            return new StorageConfigurator(configurator);
        }
    }
}
