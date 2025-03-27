using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;
using StreamStore.Sql.Tests.Database;
using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.Configuration.MultiTenant
{
    public abstract class MultitenantConfiguratorTestEnvironmentBase : TestEnvironmentBase
    {
        public SqlMultiTenantDatabaseConfigurator CreateSqlDatabaseConfigurator(IServiceCollection collection)
        {
            return new SqlMultiTenantDatabaseConfigurator(collection, DefaultConfiguration);
        }

        public abstract SqlDatabaseConfiguration DefaultConfiguration { get; }
        public abstract string SectionName { get; }

        public abstract void UseDatabase(IMultitenancyConfigurator configurator, Action<SqlMultiTenantDatabaseConfigurator> configureDatabase);
        public abstract void UseDatabaseWithAppSettings(IMultitenancyConfigurator configurator, IConfigurationRoot configuration);

        internal class FakeConnectionStringProvider : ISqlTenantConnectionStringProvider
        {
            public string GetConnectionString(Id tenantId)
            {
                return String.Empty;
            }
        }
    }
}
