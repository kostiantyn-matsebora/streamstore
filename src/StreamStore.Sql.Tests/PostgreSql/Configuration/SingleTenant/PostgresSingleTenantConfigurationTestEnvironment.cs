//using Microsoft.Extensions.Configuration;
//using StreamStore.Sql.Configuration;
//using StreamStore.Sql.PostgreSql;

//using StreamStore.Sql.Tests.Configuration.SingleTenant;

//namespace StreamStore.Sql.Tests.Postgres.Configuration.SingleTenant
//{
//    public class PostgresSingleTenantConfigurationTestEnvironment : SingleTenantConfiguratorTestEnvironmentBase
//    {
//        public override SqlStorageConfiguration DefaultConfiguration => PostgresConfiguration.DefaultConfiguration;

//        public override void UseParticularStorage(ISingleTenantConfigurator configurator, Action<SqlSingleTenantStorageConfigurator> configureStorage)
//        {
//            configurator.UsePostgresStorage(configureStorage);
//        }

//        public override void UseParticularStorageWithAppSettings(ISingleTenantConfigurator configurator, IConfigurationRoot configuration)
//        {
//            configurator.UsePostgresStorage(configuration);
//        }

//        public override string SectionName => PostgresConfiguration.ConfigurationSection;
//    }
//}
