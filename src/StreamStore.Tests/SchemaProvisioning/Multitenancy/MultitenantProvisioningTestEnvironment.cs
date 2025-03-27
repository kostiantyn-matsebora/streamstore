using Moq;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using StreamStore.Testing.Framework;

namespace StreamStore.Tests.SchemaProvisioning.Multitenancy
{
    public class MultitenantProvisioningTestEnvironment: TestEnvironmentBase
    {
        public readonly Id[] tenants = new[]
        {
            new Id("tenant-1"),
            new Id("tenant-2"),
            new Id("tenant-3"),
        };

        public Mock<ITenantSchemaProvisionerFactory> SchemaProvisionerFactory => MockRepository.Create<ITenantSchemaProvisionerFactory>();
        public ITenantProvider TenantProvider => new DefaultTenantProvider(tenants);

        public Mock<ISchemaProvisioner> MockSchemaProvisioner => MockRepository.Create<ISchemaProvisioner>();

        public readonly MockRepository MockRepository = new MockRepository(MockBehavior.Strict);
    }
}
