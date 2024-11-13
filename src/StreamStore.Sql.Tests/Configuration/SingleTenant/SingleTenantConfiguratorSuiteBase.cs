using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;

using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.Configuration.SingleTenant
{
    public abstract class SingleTenantConfiguratorSuiteBase : TestSuiteBase
    {
        public SqlSingleTenantDatabaseConfigurator CreateSqlDatabaseConfigurator(IServiceCollection collection) 
        {
            return new SqlSingleTenantDatabaseConfigurator(collection, DefaultConfiguration);
        }

        public abstract SqlDatabaseConfiguration DefaultConfiguration { get; }
        public abstract string SectionName { get; }

        public abstract void UseDatabase(ISingleTenantDatabaseConfigurator configurator, Action<SqlSingleTenantDatabaseConfigurator> configureDatabase);
        public abstract void UseDatabaseWithAppSettings(ISingleTenantDatabaseConfigurator configurator, IConfigurationRoot configuration);
    }
}
