using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Configuration.SingleTenant;

namespace StreamStore.Sql.Tests.Sqlite.Configuration.SingleTenant
{
    public class SqliteSingleTenantConfigurationTestEnvironment : SingleTenantConfiguratorTestEnvironmentBase
    {
        public override SqlDatabaseConfiguration DefaultConfiguration => SqliteConfiguration.DefaultConfiguration;

        public override string SectionName => SqliteConfiguration.ConfigurationSection;

        public override void UseParticularDatabase(ISingleTenantConfigurator configurator, Action<SqlSingleTenantDatabaseConfigurator> configureDatabase)
        {
            configurator.UseSqliteDatabase(configureDatabase);
        }

        public override void UseParticularDatabaseWithAppSettings(ISingleTenantConfigurator configurator, IConfigurationRoot configuration)
        {
            configurator.UseSqliteDatabase(configuration);
        }
    }
}
