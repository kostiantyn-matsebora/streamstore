using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using System;

namespace StreamStore
{
    public interface ISingleTenantConfigurator: IStreamStorageConfigurator
    {
        ISingleTenantConfigurator UseStorage<TStorage>(Action<IServiceCollection>? dependencies = null) where TStorage : IStreamStorage;
        ISingleTenantConfigurator UseStorage(IStreamStorage storage);

        ISingleTenantConfigurator UseSchemaProvisioner<TProvisioner>(Action<IServiceCollection>? dependencies = null) where TProvisioner : ISchemaProvisioner;
    }
}
