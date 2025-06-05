using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;


namespace StreamStore.Storage.Configuration
{
    public abstract class MultitenancyConfiguratorBase
    {
        protected abstract void ConfigureStorageProvider(StorageProviderRegistrator registrator);

        protected abstract void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator);
       

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
