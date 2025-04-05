using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.CosmosDb.Queries
{
    public class Getting_queries : Scenario
    {
        [Fact]
        public void When_any_argument_is_not_set()
        {


            // Act
            var act = () => new CosmosDbCqlQueries(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_getting_actual_revision_query()
        {

            // Arrange
            var streamId = Generated.Primitives.String;
            var queries = new CosmosDbCqlQueries(new CassandraStorageConfiguration());

            // Act
            var result = queries.StreamActualRevision(streamId);

            // Assert
            result.Should().NotBeNull();
            result.Statement.Should().NotBeNullOrEmpty();
            result.Arguments.Should().Contain(streamId);
        }
    }
}
