using StreamStore.Testing;
using StreamStore.Storage.Multitenancy;
using FluentAssertions;
using StreamStore.Provisioning;
using Moq;

namespace StreamStore.Tests.Multitenancy
{
    public class Create_schema_provisioner: Scenario
    {

        [Fact]
        public void When_delegate_is_not_defined()
        {
            var act = () => new DelegateSchemaProvisionerFactory(null!);
            act.Should().Throw<ArgumentNullException>().WithParameterName("factory");
        }

        [Fact]
        public void Should_invoke_delegate_when_creating_provisioner()
        {
            // Arrange
            var tenantId = Generated.Primitives.Id;
            var provisioner = Generated.Mocks.Single<ISchemaProvisioner>();
            var factory = new DelegateSchemaProvisionerFactory(_ => provisioner.Object);

            // Act
            var createdProvisioner = factory.Create(tenantId);

            // Assert
            createdProvisioner.Should().Be(provisioner.Object);
        }
    }
}
