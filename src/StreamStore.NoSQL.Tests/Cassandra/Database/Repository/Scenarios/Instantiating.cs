using Cassandra.Mapping;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.API;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.Repository
{
    public class Instantiating : Scenario
    {
        [Fact]
        public void When_any_argument_is_not_set()
        {
            // Act
            var act = () => new CassandraStreamRepository(null!, Generated.MockOf<ICassandraMapperFactory>().Object, new CassandraStorageConfiguration());

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new CassandraStreamRepository(Generated.MockOf<ICassandraSessionFactory>().Object, null!, new CassandraStorageConfiguration());

            // Assert
            act.Should().Throw<ArgumentNullException>();

            // Act
            act = () => new CassandraStreamRepository(Generated.MockOf<ICassandraSessionFactory>().Object, Generated.MockOf<ICassandraMapperFactory>().Object, null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
