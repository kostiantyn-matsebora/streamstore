using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Storage.Provisioning;


namespace StreamStore.Storage.Configuration
{
    public abstract class MultitenancyConfiguratorBase
    {
        protected abstract void ConfigureStorageProvider(StorageProviderRegistrator registrator);
        protected virtual void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator)
        {
            // Default implementation does nothing, can be overridden in derived classes
            registrator.RegisterSchemaProvisioningFactory((provider) => NoopSchemaProvisionerFactory.Create);
        }


        protected virtual void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            // Default implementation does nothing, can be overridden in derived classes
        }

        public void Configure(IServiceCollection services)
        {
            services.ThrowIfNull(nameof(services));
            ConfigureStorageProvider(new StorageProviderRegistrator(services));
            ConfigureSchemaProvisionerFactory(new SchemaProvisionerFactoryRegistrator(services));
            ConfigureAdditionalDependencies(services);
        }
    }
}
