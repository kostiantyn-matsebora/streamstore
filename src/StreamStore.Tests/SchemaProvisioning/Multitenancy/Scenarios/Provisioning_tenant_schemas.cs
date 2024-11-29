
using FluentAssertions;
using Moq;
using StreamStore.Provisioning;
using StreamStore.Testing;

namespace StreamStore.Tests.SchemaProvisioning.Multitenancy
{
    public class Provisioning_tenant_schemas: Scenario<MultitenantProvisioningSuite>
    {
        [Fact]
        public void When_any_of_parameters_is_not_defined()
        {
            // Act
            var act = () => new TenantSchemaProvisioningService(null!, Suite.TenantProvider);

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new TenantSchemaProvisioningService(Suite.SchemaProvisionerFactory.Object, null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task When_provisioning_all_tenants()
        {
            // Arrange
            var token = CancellationToken.None;
            var factory = Suite.SchemaProvisionerFactory;
            var tenantProvider = Suite.TenantProvider;
            var service = new TenantSchemaProvisioningService(factory.Object, tenantProvider);

            foreach (var tenant in Suite.tenants)
            {
                var provisioner = Suite.MockSchemaProvisioner;
                provisioner
                    .Setup(provisioner => provisioner.ProvisionSchemaAsync(It.IsAny<CancellationToken>()))
                    .Returns(Task.CompletedTask);

                factory
                    .Setup(factory => factory.Create(tenant))
                    .Returns(provisioner.Object);
            }
           
            // Act
            await service.StartAsync(token);

            // Assert
            Suite.MockRepository.VerifyAll();
        }
    }
}
