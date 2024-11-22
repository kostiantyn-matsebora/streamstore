﻿using FluentAssertions;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.Mocking.UnitOfWork;
public class Instantiating : Scenario
{
    [Fact]
    public void When_any_of_parameters_is_not_set()
    {

        // Act
        var act = () => new CassandraStreamUnitOfWork(Generated.Id, Generated.Revision, null, null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();

    }
}
