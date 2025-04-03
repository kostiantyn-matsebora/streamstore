using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;
using StreamStore.Sql.Tests.Storage;
using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.Configuration.MultiTenant
{
    public abstract class MultitenantConfiguratorTestEnvironmentBase : TestEnvironmentBase
    {
        public SqlMultiTenantStorageConfigurator CreateSqlStorageConfigurator(IServiceCollection collection)
        {
            return new SqlMultiTenantStorageConfigurator(collection, DefaultConfiguration);
        }

        public abstract SqlStorageConfiguration DefaultConfiguration { get; }
        public abstract string SectionName { get; }

        public abstract void UseStorage(IMultitenancyConfigurator configurator, Action<SqlMultiTenantStorageConfigurator> configureStorage);
        public abstract void UseStorageWithAppSettings(IMultitenancyConfigurator configurator, IConfigurationRoot configuration);

        internal class FakeConnectionStringProvider : ISqlTenantConnectionStringProvider
        {
            public string GetConnectionString(Id tenantId)
            {
                return String.Empty;
            }
        }
    }
}
