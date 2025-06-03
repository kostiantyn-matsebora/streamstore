using StreamStore.Provisioning;

namespace StreamStore.Storage.Provisioning
{
    internal class NoopSchemaProvisionerFactory
    {
        public static ISchemaProvisioner Create(Id tenantId)
        {
            return new NoopSchemaProvisioner();
        }
    }
}
