using Cassandra.Mapping;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Repository
{
    public class Instantiating : Scenario
    {
        [Fact]
        public void When_any_parameter_is_not_set()
        {
            // Act
            var act = () => new CassandraStreamRepository(null!,  new CassandraStorageConfiguration(), new MappingConfiguration());

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new CassandraStreamRepository(Generated.MockOf<ICassandraSessionFactory>().Object, null!, new MappingConfiguration());

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new CassandraStreamRepository(Generated.MockOf<ICassandraSessionFactory>().Object, new CassandraStorageConfiguration(), null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();

        }
    }
}
