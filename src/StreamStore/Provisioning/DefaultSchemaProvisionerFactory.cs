
namespace StreamStore.Provisioning
{
    internal class DefaultSchemaProvisionerFactory : ITenantSchemaProvisionerFactory
    {
        public ISchemaProvisioner Create(Id tenantId)
        {
            return new DefaultSchemaProvisioner();
        }
    }
}
