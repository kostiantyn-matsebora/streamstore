using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Configuration.MultiTenant;


namespace StreamStore.Sql.Tests.Sqlite.Configuration.Multitenancy
{
    public class SqliteMultiTenantConfigurationSuite : MultiTenantConfiguratorSuiteBase
    {
        public override SqlDatabaseConfiguration DefaultConfiguration => SqliteConfiguration.DefaultConfiguration;

        public override string SectionName => SqliteConfiguration.ConfigurationSection;

        public override void UseDatabase(IMultitenantDatabaseConfigurator configurator, Action<SqlMultiTenantDatabaseConfigurator> configureDatabase)
        {
            configurator.UseSqliteDatabase(x =>
            {
                x.WithConnectionStringProvider<FakeConnectionStringProvider>();
                configureDatabase(x);
            });
        }

        public override void UseDatabaseWithAppSettings(IMultitenantDatabaseConfigurator configurator, IConfigurationRoot configuration)
        {

            configurator.UseSqliteDatabase(configuration, x => x.WithConnectionStringProvider<FakeConnectionStringProvider>());
        }
    }
}
