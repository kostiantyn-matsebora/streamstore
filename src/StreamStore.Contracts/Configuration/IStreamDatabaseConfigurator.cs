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

    public interface ISingleTenantDatabaseConfigurator: IStreamDatabaseConfigurator
    {
        public ISingleTenantDatabaseConfigurator UseDatabase<TDatabase>(Action<IServiceCollection>? dependencies = null) where TDatabase : IStreamDatabase;
        public ISingleTenantDatabaseConfigurator UseDatabase(IStreamDatabase database);

        ISingleTenantDatabaseConfigurator UseSchemaProvisioner<TProvisioner>(Action<IServiceCollection>? dependencies = null) where TProvisioner : ISchemaProvisioner;
    }

    public interface IMultitenantDatabaseConfigurator : IStreamDatabaseConfigurator
    {
        IMultitenantDatabaseConfigurator UseDatabaseProvider<TDatabaseProvider>(Action<IServiceCollection>? dependencies = null) where TDatabaseProvider : ITenantStreamDatabaseProvider;
        IMultitenantDatabaseConfigurator UseSchemaProvisionerFactory<TFactory>(Action<IServiceCollection>? dependencies = null) where TFactory : ITenantSchemaProvisionerFactory;
    }
}
