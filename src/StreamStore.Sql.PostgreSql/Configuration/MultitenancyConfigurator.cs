using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Sql.Multitenancy;
using StreamStore.Storage;
using StreamStore.Storage.Configuration;

namespace StreamStore.Sql.PostgreSql
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
            registrator.RegisterStorageProvider(serviceProvider => serviceProvider.GetRequiredService<PostgresTenantStorageProvider>().GetStorage);
        }

        protected override void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator)
        {
            registrator.RegisterSchemaProvisioningFactory(provider =>
                provider.GetRequiredService<PostgresSchemaProvisionerFactory>().Create);
        }


        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            services.ThrowIfNull(nameof(services));
            services.AddSingleton<ISqlTenantStorageConfigurationProvider, SqlTenantStorageConfigurationProvider>();
            services.AddSingleton<PostgresTenantStorageProvider>();
            configure(new SqlMultitenancyConfigurator(services));
            services.AddSingleton<PostgresSchemaProvisionerFactory>();
        }
    }
}