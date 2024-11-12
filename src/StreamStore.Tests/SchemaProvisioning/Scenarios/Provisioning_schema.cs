using FluentAssertions;
using Moq;
using StreamStore.Provisioning;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.SchemaProvisioning
{
    public class Provisioning_schema: Scenario<SchemaProvisiningSuite>
    {
        [Fact]
        public void When_provisioner_is_not_set()
        {

            // Act
            var act = () => new SchemaProvisioningService(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task When_provisioning_schema()
        {
            // Arrange
            var provisioningService = new SchemaProvisioningService(Suite.MockProvisioner.Object);
            var token = CancellationToken.None;

            Suite.MockProvisioner
                .Setup(provisioner => provisioner.ProvisionSchemaAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
            // Act
            await provisioningService.StartAsync(token);

            // Assert
            Suite.MockRepository.VerifyAll();
        }
    }
}
