using Microsoft.Extensions.DependencyInjection;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using System;

namespace StreamStore
{
    public interface IStreamDatabaseConfigurator
    {
        public IServiceCollection Configure();
    }

    public interface ISingleTenantConfigurator: IStreamDatabaseConfigurator
    {
        ISingleTenantConfigurator UseDatabase<TDatabase>(Action<IServiceCollection>? dependencies = null) where TDatabase : IStreamStorage;
        ISingleTenantConfigurator UseDatabase(IStreamStorage database);

        ISingleTenantConfigurator UseSchemaProvisioner<TProvisioner>(Action<IServiceCollection>? dependencies = null) where TProvisioner : ISchemaProvisioner;
    }

    public interface IMultitenancyConfigurator : IStreamDatabaseConfigurator
    {
        IMultitenancyConfigurator UseDatabaseProvider<TDatabaseProvider>(Action<IServiceCollection>? dependencies = null) where TDatabaseProvider : ITenantStreamDatabaseProvider;
        IMultitenancyConfigurator UseSchemaProvisionerFactory<TFactory>(Action<IServiceCollection>? dependencies = null) where TFactory : ITenantSchemaProvisionerFactory;

        IMultitenancyConfigurator UseTenantProvider<TProvider>(Action<IServiceCollection>? dependencies = null) where TProvider : ITenantProvider;
        IMultitenancyConfigurator WithTenants(params Id[] tenants);
    }
}
