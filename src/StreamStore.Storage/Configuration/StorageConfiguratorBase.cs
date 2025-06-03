using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Storage.Provisioning;


namespace StreamStore.Storage.Configuration
{
    public abstract class StorageConfiguratorBase
    {
        protected abstract void ConfigureStorage(StorageDependencyRegistrator registrator);
        protected virtual void ConfigureSchemaProvisioner(SchemaProvisionerRegistrator registrator)
        {
            // Default implementation does nothing, can be overridden in derived classes
            registrator.RegisterSchemaProvisioner<NoopSchemaProvisioner>();
        }

        protected virtual void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            // Default implementation does nothing, can be overridden in derived classes
        }

        public void Configure(IServiceCollection services)
        {
            services.ThrowIfNull(nameof(services));
            ConfigureStorage(new StorageDependencyRegistrator(services));
            ConfigureSchemaProvisioner(new SchemaProvisionerRegistrator(services));
            ConfigureAdditionalDependencies(services);
        }
    }
}
