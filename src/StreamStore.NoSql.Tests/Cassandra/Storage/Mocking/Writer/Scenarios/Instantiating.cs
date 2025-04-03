using FluentAssertions;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Storage.Mocking.UnitOfWork;
public class Instantiating : Scenario
{
    [Fact]
    public void When_any_of_parameters_is_not_set()
    {

        // Act
        var act = () => new CassandraStreamWriter(Generated.Primitives.Id, Generated.Primitives.Revision, null, null!, null!, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();

    }
}

