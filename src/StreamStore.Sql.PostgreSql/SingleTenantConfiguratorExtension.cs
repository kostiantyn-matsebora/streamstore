//using System;
//using Microsoft.Extensions.Configuration;
//using StreamStore.Sql.Configuration;

//namespace StreamStore.Sql.PostgreSql
//{
//    public static class SingleTenantConfiguratorExtension
//    {
//        const string configurationSection = "StreamStore:PostgreSql";

//        public static ISingleTenantDatabaseConfigurator UsePostgresDatabase(this ISingleTenantDatabaseConfigurator configurator, IConfiguration configuration)
//        {
//            return configurator.UseSqlDatabase(DefaultConfiguration, configuration, configurationSection, ConfigureRequiredDependencies);

//        }

//        public static ISingleTenantDatabaseConfigurator UsePostgresDatabase(
//                        this ISingleTenantDatabaseConfigurator configurator, 
//                        Action<SqlSingleTenantDatabaseConfigurator> dbConfigurator)
//        {
//            return configurator.UseSqlDatabase(DefaultConfiguration, (c) =>
//            {
//                ConfigureRequiredDependencies(c);
//                dbConfigurator(c);
//            });
//        }

//        static void ConfigureRequiredDependencies(SqlSingleTenantDatabaseConfigurator configurator)
//        {
//            configurator.WithConnectionFactory<PostgresConnectionFactory>();
//            configurator.WithExceptionHandling<PostgresExceptionHandler>();
//            configurator.WithProvisioingQueryProvider<PostgresProvisioningQueryProvider>();
//        }


//        static SqlDatabaseConfiguration DefaultConfiguration = new SqlDatabaseConfiguration
//        {
//            SchemaName = "public",
//            TableName = "Events",
//        };
//    }
//}
