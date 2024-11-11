using Moq;
using StreamStore.Sql.API;
using StreamStore.Testing.Framework;

namespace StreamStore.Sql.Tests.SchemaProvisioning
{
    public class SchemaProvisiningSuite: TestSuiteBase
    {
        public SchemaProvisiningSuite()
        {
            MockRepository = new MockRepository(MockBehavior.Default);

            MockProvisioner = MockRepository.Create<ISqlSchemaProvisioner>();
        }

        public MockRepository MockRepository { get; }
        public Mock<ISqlSchemaProvisioner> MockProvisioner { get; }
    }
}
