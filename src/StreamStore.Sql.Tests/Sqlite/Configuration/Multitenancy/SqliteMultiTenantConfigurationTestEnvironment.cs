using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Configuration.MultiTenant;


namespace StreamStore.Sql.Tests.Sqlite.Configuration.Multitenancy
{
    public class SqliteMultiTenantConfigurationTestEnvironment : MultitenantConfiguratorTestEnvironmentBase
    {
        public override SqlStorageConfiguration DefaultConfiguration => SqliteConfiguration.DefaultConfiguration;

        public override string SectionName => SqliteConfiguration.ConfigurationSection;

        public override void UseStorage(IMultitenancyConfigurator configurator, Action<SqlMultiTenantStorageConfigurator> configureStorage)
        {
            configurator.UseSqliteStorage(x =>
            {
                x.WithConnectionStringProvider<FakeConnectionStringProvider>();
                configureStorage(x);
            });
        }

        public override void UseStorageWithAppSettings(IMultitenancyConfigurator configurator, IConfigurationRoot configuration)
        {

            configurator.UseSqliteStorage(configuration, x => x.WithConnectionStringProvider<FakeConnectionStringProvider>());
        }
    }
}
