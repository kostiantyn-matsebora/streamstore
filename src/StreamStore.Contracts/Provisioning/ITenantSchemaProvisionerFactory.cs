namespace StreamStore.Provisioning
{
    public interface ITenantSchemaProvisionerFactory
    {
        ISchemaProvisioner Create(Id tenantId);
    }
}
