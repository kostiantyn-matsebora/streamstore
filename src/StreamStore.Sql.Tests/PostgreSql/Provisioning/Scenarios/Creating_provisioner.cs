using FluentAssertions;
using StreamStore.Sql.Configuration;
using StreamStore.Sql.Multitenancy;
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
            var act = () => new PostgresSchemaProvisionerFactory(null!, Generated.Objects.Single<MigrationConfiguration>());

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new PostgresSchemaProvisionerFactory(new SqlTenantStorageConfigurationProvider(Generated.Objects.Single<SqlStorageConfiguration>(), new SqlDefaultConnectionStringProvider()), null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_creating_tenant_provisioner()
        {
            // Arrange
            var configurationProvider = Environment.MockSqlConfigurationProvider;
            var tenantId = Generated.Primitives.Id;
            var factory = new PostgresSchemaProvisionerFactory(configurationProvider.Object, Generated.Objects.Single<MigrationConfiguration>());

            configurationProvider
                .Setup(provider => provider.GetConfiguration(tenantId))
                .Returns(new SqlStorageConfiguration());

            // Act
            var provisioner = factory.Create(tenantId);

            // Assert
            provisioner.Should().NotBeNull().And.BeOfType<SqlSchemaProvisioner>();
        }
    }
}
