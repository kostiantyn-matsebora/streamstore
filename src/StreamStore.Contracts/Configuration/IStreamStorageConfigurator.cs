using Microsoft.Extensions.DependencyInjection;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using System;

namespace StreamStore
{
    public interface IStreamStorageConfigurator
    {
        public IServiceCollection Configure();
    }

    public interface ISingleTenantConfigurator: IStreamStorageConfigurator
    {
        ISingleTenantConfigurator UseStorage<TStorage>(Action<IServiceCollection>? dependencies = null) where TStorage : IStreamStorage;
        ISingleTenantConfigurator UseStorage(IStreamStorage storage);

        ISingleTenantConfigurator UseSchemaProvisioner<TProvisioner>(Action<IServiceCollection>? dependencies = null) where TProvisioner : ISchemaProvisioner;
    }

    public interface IMultitenancyConfigurator : IStreamStorageConfigurator
    {
        IMultitenancyConfigurator UseStorageProvider<TStorageProvider>(Action<IServiceCollection>? dependencies = null) where TStorageProvider : ITenantStreamStorageProvider;
        IMultitenancyConfigurator UseSchemaProvisionerFactory<TFactory>(Action<IServiceCollection>? dependencies = null) where TFactory : ITenantSchemaProvisionerFactory;

        IMultitenancyConfigurator UseTenantProvider<TProvider>(Action<IServiceCollection>? dependencies = null) where TProvider : ITenantProvider;
        IMultitenancyConfigurator WithTenants(params Id[] tenants);
    }
}
