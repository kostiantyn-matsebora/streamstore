using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;


namespace StreamStore.Storage.Configuration
{
    public abstract class MultitenancyConfiguratorBase: IMultitenancyConfigurator
    {
        protected abstract void ConfigureStorageProvider(StorageProviderRegistrator registrator);

        protected abstract void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator);


        protected abstract void ConfigureAdditionalDependencies(IServiceCollection services);

        public void Configure(IServiceCollection services)
        {
            services.ThrowIfNull(nameof(services));
            ConfigureStorageProvider(new StorageProviderRegistrator(services));
            ConfigureSchemaProvisionerFactory(new SchemaProvisionerFactoryRegistrator(services));
            ConfigureAdditionalDependencies(services);
        }
    }
}
