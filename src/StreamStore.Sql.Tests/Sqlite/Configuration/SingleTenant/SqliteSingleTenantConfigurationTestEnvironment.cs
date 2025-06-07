//using Microsoft.Extensions.Configuration;
//using StreamStore.Sql.Configuration;
//using StreamStore.Sql.Sqlite;
//using StreamStore.Sql.Tests.Configuration.SingleTenant;

//namespace StreamStore.Sql.Tests.Sqlite.Configuration.SingleTenant
//{
//    public class SqliteSingleTenantConfigurationTestEnvironment : SingleTenantConfiguratorTestEnvironmentBase
//    {
//        public override SqlStorageConfiguration DefaultConfiguration => SqliteConfiguration.DefaultConfiguration;

//        public override string SectionName => SqliteConfiguration.ConfigurationSection;

//        public override void UseParticularStorage(ISingleTenantConfigurator configurator, Action<SqlSingleTenantStorageConfigurator> configureStorage)
//        {
//            configurator.UseSqliteStorage(configureStorage);
//        }

//        public override void UseParticularStorageWithAppSettings(ISingleTenantConfigurator configurator, IConfigurationRoot configuration)
//        {
//            configurator.UseSqliteStorage(configuration);
//        }
//    }
//}
