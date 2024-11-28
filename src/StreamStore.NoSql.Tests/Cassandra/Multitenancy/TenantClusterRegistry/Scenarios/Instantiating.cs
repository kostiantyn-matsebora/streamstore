using FluentAssertions;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.TenantClusterRegistry
{
    public class Instantiating: Scenario
    {
        [Fact]
        public void When_any_of_parameters_is_not_set()
        {

            // Act
            var act = () => new CassandraTenantClusterRegistry(new DelegateClusterConfigurator(), null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new CassandraTenantClusterRegistry(null!, new DelegateTenantClusterConfigurator());

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
