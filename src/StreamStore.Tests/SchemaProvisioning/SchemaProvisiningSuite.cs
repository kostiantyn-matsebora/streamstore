using Moq;
using StreamStore.Provisioning;
using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.SchemaProvisioning
{
    public class SchemaProvisiningSuite: TestSuiteBase
    {
        public SchemaProvisiningSuite()
        {
            MockRepository = new MockRepository(MockBehavior.Default);

            MockProvisioner = MockRepository.Create<ISchemaProvisioner>();
        }

        public MockRepository MockRepository { get; }
        public Mock<ISchemaProvisioner> MockProvisioner { get; }
    }
}
