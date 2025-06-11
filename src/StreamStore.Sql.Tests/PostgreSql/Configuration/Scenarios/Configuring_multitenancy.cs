using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.PostgreSql;
using StreamStore.Storage;
using StreamStore.Testing.Storage.Configuration;

namespace StreamStore.Sql.Tests.PostgreSql.Configuration
{
    public class Configuring_multitenancy: MultitenancyConfiguratorScenario
    {
        protected override IMultitenancyConfigurator CreateConfigurator()
        {
            return new MultitenancyConfigurator((c) => { });
        }

        protected override void ConfigureRequiredDependencies(IServiceCollection services)
        {
            services.AddSingleton(new SqlStorageConfiguration());
            services.AddSingleton(new MigrationConfiguration());
        }
    }
}
