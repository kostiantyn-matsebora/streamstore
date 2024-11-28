using FluentAssertions;
using StreamStore.NoSql.Cassandra.Multitenancy;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Multitenancy.MappingRegistry.Scenarios;

public class Instantiating: Scenario
{
	[Fact]
	public void When_any_argument_is_not_set()
	{
		// Act
		var act = () => new CassandraTenantMappingRegistry(null!);

		// Assert
		act.Should().Throw<ArgumentNullException>();
	}
}
