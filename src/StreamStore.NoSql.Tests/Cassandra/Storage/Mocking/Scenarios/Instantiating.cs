using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Storage.Mocking
{
    public class Instantiating : Scenario
    {

        [Fact]
        public void When_any_argument_is_not_set()
        {
            //Act
            var act = () => new CassandraStreamStorage(null!, Generated.Mocks.Single<ICassandraCqlQueries>().Object, new CassandraStorageConfiguration());

            //Assert
            act.Should().Throw<ArgumentNullException>();

            //Act
            act = () => new CassandraStreamStorage(Generated.Mocks.Single<ICassandraMapperProvider>().Object, null!, new CassandraStorageConfiguration());

            //Assert
            act.Should().Throw<ArgumentNullException>();

            //Act
            act = () => new CassandraStreamStorage(Generated.Mocks.Single<ICassandraMapperProvider>().Object, Generated.Mocks.Single<ICassandraCqlQueries>().Object, null!);

            //Assert
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
