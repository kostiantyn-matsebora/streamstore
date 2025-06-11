using StreamStore.Sql.Sqlite;
using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;

namespace StreamStore.Sql.Tests.Sqlite.Configuration
{
    public class Configuring_storage : StorageConfiguratorScenario
    {
        protected override IStorageConfigurator CreateConfigurator()
        {
            return new StorageConfigurator();
        }
    }
}
