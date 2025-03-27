using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Tests.Configuration.MultiTenant;


namespace StreamStore.Sql.Tests.Postgres.Configuration.Multitenancy
{
    public class PostgresMultiTenantConfigurationTestEnvironment : MultitenantConfiguratorTestEnvironmentBase
    {
        public override SqlDatabaseConfiguration DefaultConfiguration => PostgresConfiguration.DefaultConfiguration;

        public override string SectionName => PostgresConfiguration.ConfigurationSection;

        public override void UseDatabase(IMultitenancyConfigurator configurator, Action<SqlMultiTenantDatabaseConfigurator> configureDatabase)
        {
            configurator.UsePostgresDatabase(x =>
            {
                x.WithConnectionStringProvider<FakeConnectionStringProvider>();
                configureDatabase(x);
            });
        }

        public override void UseDatabaseWithAppSettings(IMultitenancyConfigurator configurator, IConfigurationRoot configuration)
        {
            configurator.UsePostgresDatabase(configuration, x => x.WithConnectionStringProvider<FakeConnectionStringProvider>());
        }
    }
}
