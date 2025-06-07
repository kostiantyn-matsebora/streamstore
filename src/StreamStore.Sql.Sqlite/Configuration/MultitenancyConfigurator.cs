using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Sql.Multitenancy;
using StreamStore.Storage.Configuration;

namespace StreamStore.Sql.Sqlite
{
    internal class MultitenancyConfigurator : MultitenancyConfiguratorBase
    {
        readonly Action<SqlMultitenancyConfigurator> configure;

        public MultitenancyConfigurator(Action<SqlMultitenancyConfigurator> configure)
        {
            this.configure = configure.ThrowIfNull(nameof(configure));
        }

        protected override void ConfigureStorageProvider(StorageProviderRegistrator registrator)
        {
            registrator.RegisterStorageProvider(serviceProvider => serviceProvider.GetRequiredService<SqliteTenantStorageProvider>().GetStorage);
        }

        protected override void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator)
        {
            registrator.RegisterSchemaProvisioningFactory(provider =>
                provider.GetRequiredService<SqliteSchemaProvisionerFactory>().Create);
        }

        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            services.ThrowIfNull(nameof(services));
            services.AddSingleton<ISqlTenantStorageConfigurationProvider,SqlTenantStorageConfigurationProvider>();
            services.AddSingleton<SqliteTenantStorageProvider>();
            configure(new SqlMultitenancyConfigurator(services));
            services.AddSingleton<SqliteSchemaProvisionerFactory>();
        }
    }
}