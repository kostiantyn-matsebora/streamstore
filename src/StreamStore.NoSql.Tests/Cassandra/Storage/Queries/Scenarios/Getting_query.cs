﻿using FluentAssertions;
using StreamStore.NoSql.Cassandra.Configuration;
using StreamStore.NoSql.Cassandra.Storage;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Storage.Queries
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
            var streamId = Generated.Primitives.String;

            // Act
            var cql = queries.StreamMetadata(streamId);

            // Assert
            cql.Should().NotBeNull();
            cql.Statement.Should().NotBeNullOrWhiteSpace();
            cql.Arguments.Should().Contain(streamId);
        }

        [Fact]
        public void When_getting_delete_stream_query()
        {

            // Arrange
            var queries = new CassandraCqlQueries(new CassandraStorageConfiguration());
            var streamId = Generated.Primitives.String;

            // Act
            var cql = queries.DeleteStream(streamId);

            // Assert
            cql.Should().NotBeNull();
            cql.Statement.Should().NotBeNullOrWhiteSpace();
            cql.Arguments.Should().Contain(streamId);
        }

        [Fact]
        public void When_getting_stream_events_query()
        {

            // Arrange
            var queries = new CassandraCqlQueries(new CassandraStorageConfiguration());
            var streamId = Generated.Primitives.String;
            var startFrom = Generated.Primitives.Int;
            var count = Generated.Primitives.Int;

            // Act
            var cql = queries.StreamEvents(streamId, startFrom, count);

            // Assert
            cql.Should().NotBeNull();
            cql.Statement.Should().NotBeNullOrWhiteSpace();
            cql.Arguments.Should().Contain(streamId);
            cql.Arguments.Should().Contain(startFrom);
            cql.Arguments.Should().Contain(count);
        }
    }
}
