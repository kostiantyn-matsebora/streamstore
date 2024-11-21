using FluentAssertions;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.Mocking
{
    public class Instantiating : Scenario
    {

        [Fact]
        public void When_repository_factory_not_set()
        {
            //Act
            var act = () => new CassandraStreamDatabase(null!);

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_repository_factory_set()
        {
            //Act
            var act = () => new CassandraStreamDatabase(Generated.MockOf<ICassandraStreamRepositoryFactory>().Object);

            //Assert
            act.Should().NotThrow();
        }

    }
}
