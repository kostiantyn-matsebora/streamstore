using FluentAssertions;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.PostgreSql;
using StreamStore.Sql.Provisioning;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.PostgreSql.Provisioning
{
    public class Creating_provisioner: Scenario<PostgresProvisioningTestEnvironment>
    {
        [Fact]
        public void When_any_argument_is_not_defined()
        {

            // Act
            var act = () => new PostgresSchemaProvisionerFactory(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_creating_tenant_provisioner()
        {
            // Arrange
            var configurationProvider = Environment.MockSqlConfigurationProvider;
            var tenantId = Generated.Primitives.Id;
            var factory = new PostgresSchemaProvisionerFactory(configurationProvider.Object);

            configurationProvider
                .Setup(provider => provider.GetConfiguration(tenantId))
                .Returns(new SqlDatabaseConfiguration());

            // Act
            var provisioner = factory.Create(tenantId);

            // Assert
            provisioner.Should().NotBeNull().And.BeOfType<SqlSchemaProvisioner>();
        }
    }
}
