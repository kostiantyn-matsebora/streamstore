//using Microsoft.Extensions.Configuration;
//using StreamStore.Sql.Configuration;
//using StreamStore.Sql.PostgreSql;
//using StreamStore.Sql.Tests.Configuration.MultiTenant;


//namespace StreamStore.Sql.Tests.Postgres.Configuration.Multitenancy
//{
//    public class PostgresMultiTenantConfigurationTestEnvironment : MultitenantConfiguratorTestEnvironmentBase
//    {
//        public override SqlStorageConfiguration DefaultConfiguration => PostgresConfiguration.DefaultConfiguration;

//        public override string SectionName => PostgresConfiguration.ConfigurationSection;

//        public override void UseStorage(IMultitenancyConfigurator configurator, Action<SqlMultiTenantStorageConfigurator> configureStorage)
//        {
//            configurator.UsePostgresStorage(x =>
//            {
//                x.WithConnectionStringProvider<FakeConnectionStringProvider>();
//                configureStorage(x);
//            });
//        }

//        public override void UseStorageWithAppSettings(IMultitenancyConfigurator configurator, IConfigurationRoot configuration)
//        {
//            configurator.UsePostgresStorage(configuration, x => x.WithConnectionStringProvider<FakeConnectionStringProvider>());
//        }
//    }
//}
