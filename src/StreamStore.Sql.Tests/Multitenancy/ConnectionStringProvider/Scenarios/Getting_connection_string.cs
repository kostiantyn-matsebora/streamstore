using FluentAssertions;
using StreamStore.Sql.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.Sql.Tests.Multitenancy.ConnectionStringProvider
{
    public class Getting_connection_string: Scenario
    {

        [Fact]
        public void When_connection_string_for_tenant_exists()
        {
            // Arrange
            var tenantId = "tenant1";
            var connectionString = "connectionString";

            var provider = new SqlDefaultConnectionStringProvider().AddConnectionString(tenantId, connectionString);

            // Act
            var result = provider.GetConnectionString(tenantId);

            // Assert
            result.Should().Be(connectionString);
        }

        [Fact]
        public void When_connection_string_for_tenant_does_not_exist()
        {
            // Arrange
            var tenantId = "tenant1";
            var connectionString = "connectionString";

            var provider = new SqlDefaultConnectionStringProvider().AddConnectionString(tenantId, connectionString);

            // Act
            var act = () =>  provider.GetConnectionString(Generated.Id);

            // Assert
            act.Should().Throw<InvalidOperationException>();
        }
    }
}
