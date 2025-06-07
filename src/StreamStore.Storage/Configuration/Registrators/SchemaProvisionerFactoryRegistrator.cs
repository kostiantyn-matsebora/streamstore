using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Provisioning;
using StreamStore.Storage.Multitenancy;
using StreamStore.Storage.Provisioning;

namespace StreamStore.Storage.Configuration
{
    public sealed class SchemaProvisionerFactoryRegistrator
    {
        readonly IServiceCollection services;

        public SchemaProvisionerFactoryRegistrator(IServiceCollection services)
        {
          this.services = services.ThrowIfNull(nameof(services));
        }

        public IServiceCollection RegisterSchemaProvisioningFactory(Func<IServiceProvider, Func<Id, ISchemaProvisioner>>  factory)
        {
            return services.AddSingleton<ITenantSchemaProvisionerFactory>(sp => new DelegateSchemaProvisionerFactory(factory(sp)));
        }

        public IServiceCollection RegisterDummySchemaProvisioningFactory()
        {
            return RegisterSchemaProvisioningFactory((provider) => NoopSchemaProvisionerFactory.Create);
        }
    }
}
