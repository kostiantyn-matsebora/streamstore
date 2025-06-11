using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;


namespace StreamStore.Storage.Configuration
{
    public abstract class StorageConfiguratorBase: IStorageConfigurator
    {
        protected abstract void ConfigureStorage(StorageDependencyRegistrator registrator);
        protected abstract void ConfigureSchemaProvisioner(SchemaProvisionerRegistrator registrator);
        protected abstract void ConfigureAdditionalDependencies(IServiceCollection services);

        public void Configure(IServiceCollection services)
        {
            services.ThrowIfNull(nameof(services));
            ConfigureStorage(new StorageDependencyRegistrator(services));
            ConfigureSchemaProvisioner(new SchemaProvisionerRegistrator(services));
            ConfigureAdditionalDependencies(services);

        }
    }
}
