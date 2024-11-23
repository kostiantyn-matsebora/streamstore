using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.Queries.Scenarios
{
    public class Getting_query: Scenario
    {
        [Fact]
        public void When_any_argument_is_not_set()
        {
            // Act
            var act = () => new CassandraCqlQueries(null!);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void When_getting_stream_actual_revision_query()
        {

            // Arrange
            var queries = new CassandraCqlQueries(new CassandraStorageConfiguration());
            var streamId = Generated.String;

            // Act
            var cql = queries.StreamActualRevision(streamId);

            // Assert
            cql.Should().NotBeNull();
            cql.Statement.Should().NotBeNullOrWhiteSpace();
            cql.Arguments.Should().Contain(streamId);
        }
    }
}
