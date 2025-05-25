
namespace StreamStore.Provisioning
{
    class DefaultSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        public ISchemaProvisioner Create(Id tenantId)
        {
            return new DefaultSchemaProvisioner();
        }
    }
}
