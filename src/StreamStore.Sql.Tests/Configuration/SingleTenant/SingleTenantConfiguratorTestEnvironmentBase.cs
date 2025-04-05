using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;

using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.Configuration.SingleTenant
{
    public abstract class SingleTenantConfiguratorTestEnvironmentBase : TestEnvironmentBase
    {
        public SqlSingleTenantStorageConfigurator CreateSqlStorageConfigurator(IServiceCollection collection) 
        {
            return new SqlSingleTenantStorageConfigurator(collection, DefaultConfiguration);
        }

        public abstract SqlStorageConfiguration DefaultConfiguration { get; }
        public abstract string SectionName { get; }

        public abstract void UseParticularStorage(ISingleTenantConfigurator configurator, Action<SqlSingleTenantStorageConfigurator> configureStorage);
        public abstract void UseParticularStorageWithAppSettings(ISingleTenantConfigurator configurator, IConfigurationRoot configuration);
    }
}
