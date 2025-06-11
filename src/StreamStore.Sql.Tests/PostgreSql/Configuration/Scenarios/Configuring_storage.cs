using StreamStore.Sql.PostgreSql;
using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;

namespace StreamStore.Sql.Tests.PostgreSql.Configuration
{
    public class Configuring_storage : StorageConfiguratorScenario
    {
        protected override IStorageConfigurator CreateConfigurator()
        {
            return new StorageConfigurator();
        }
    }
}
