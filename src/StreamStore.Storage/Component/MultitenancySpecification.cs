using StreamStore.Configuration;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;

namespace StreamStore.Storage.Component
{
    public class MultitenancySpecification : ComponentSpecificationBase
    {
        public MultitenancySpecification(bool provisioningEnabled)
        {
            AddRequiredDependency<ITenantStreamStorageProvider>();
            if (provisioningEnabled)
                AddRequiredDependency<ITenantSchemaProvisionerFactory>();
        }
    }
}
