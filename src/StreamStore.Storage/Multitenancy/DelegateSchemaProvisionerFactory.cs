using System;
using StreamStore.Provisioning;


namespace StreamStore.Storage.Multitenancy
{
    internal class DelegateSchemaProvisionerFactory: ITenantSchemaProvisionerFactory
    {
        Func<Id, ISchemaProvisioner> factory;
        public DelegateSchemaProvisionerFactory(Func<Id, ISchemaProvisioner> factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public ISchemaProvisioner Create(Id tenantId)
        {
            return factory(tenantId);
        }
    }
}
