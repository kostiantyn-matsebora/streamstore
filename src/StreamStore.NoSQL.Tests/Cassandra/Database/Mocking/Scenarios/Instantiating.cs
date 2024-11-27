using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.Mocking
{
    public class Instantiating : Scenario
    {

        [Fact]
        public void When_any_argument_is_not_set()
        {
            //Act
            var act = () => new CassandraStreamDatabase(null!, Generated.MockOf<ICassandraCqlQueries>().Object, new CassandraStorageConfiguration());

            //Assert
            act.Should().Throw<ArgumentNullException>();

            //Act
            act = () => new CassandraStreamDatabase(Generated.MockOf<ICassandraMapperProvider>().Object, null!, new CassandraStorageConfiguration());

            //Assert
            act.Should().Throw<ArgumentNullException>();

            //Act
            act = () => new CassandraStreamDatabase(Generated.MockOf<ICassandraMapperProvider>().Object, Generated.MockOf<ICassandraCqlQueries>().Object, null!);

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
