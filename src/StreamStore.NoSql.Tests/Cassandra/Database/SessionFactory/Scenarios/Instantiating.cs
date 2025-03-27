
using Cassandra;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.SessionFactory
{
    public class Instantiating: Scenario
    {
        [Fact]
        public void When_any_argument_is_not_set()
        {
            //Act
            var act = () => new CassandraSessionFactory(Generated.Mocks.Single<ICluster>().Object, null!);

            //Assert
            act.Should().Throw<ArgumentNullException>();

            //Act
            act = () => new CassandraSessionFactory(null!, new CassandraStorageConfiguration());

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_arguments_set_properly()
        {
            // Arrange, Act
            var sessionFactory = new CassandraSessionFactory(Generated.Mocks.Single<ICluster>().Object, new CassandraStorageConfiguration());

            // Assert
            sessionFactory.Should().NotBeNull();
        }
    }
}
