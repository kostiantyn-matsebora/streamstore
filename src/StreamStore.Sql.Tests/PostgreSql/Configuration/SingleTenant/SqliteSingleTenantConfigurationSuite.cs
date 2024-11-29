using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.PostgreSql;

using StreamStore.Sql.Tests.Configuration.SingleTenant;

namespace StreamStore.Sql.Tests.Postgres.Configuration.SingleTenant
{
    public class PostgresSingleTenantConfigurationSuite : SingleTenantConfiguratorSuiteBase
    {
        public override SqlDatabaseConfiguration DefaultConfiguration => PostgresConfiguration.DefaultConfiguration;

        public override void UseParticularDatabase(ISingleTenantConfigurator configurator, Action<SqlSingleTenantDatabaseConfigurator> configureDatabase)
        {
            configurator.UsePostgresDatabase(configureDatabase);
        }

        public override void UseParticularDatabaseWithAppSettings(ISingleTenantConfigurator configurator, IConfigurationRoot configuration)
        {
            configurator.UsePostgresDatabase(configuration);
        }

        public override string SectionName => PostgresConfiguration.ConfigurationSection;
    }
}
