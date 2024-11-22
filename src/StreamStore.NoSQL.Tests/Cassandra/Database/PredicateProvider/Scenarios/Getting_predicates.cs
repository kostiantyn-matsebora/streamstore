

using System.Linq;
using FluentAssertions;
using StreamStore.NoSql.Cassandra.Database;
using StreamStore.NoSql.Cassandra.Models;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.PredicateProvider
{
    public class Getting_predicates : Scenario
    {

        [Fact]
        public void When_getting_stream_revisions_predicate()
        {
            // Arrange
            var predicateProvider = new CassandraPredicateProvider();
            var streamRevisions = Generated.CreateMany<RevisionStreamEntity>(10);
            var streamId = streamRevisions.First().StreamId;

            var expectedRevisions = streamRevisions.Where(r => r.StreamId == streamId).ToList();

            // Act
            var predicate = predicateProvider.StreamRevisions(streamId);

            // Assert
            streamRevisions.Where(predicate.Compile())
                .Should().NotBeEmpty()
                .And.BeEquivalentTo(expectedRevisions);
        }

        [Fact]
        public void When_getting_stream_metadata_predicate()
        {
            // Arrange
            var predicateProvider = new CassandraPredicateProvider();
            var metadata = Generated.CreateMany<EventMetadataEntity>(10);
            var streamId = metadata.First().StreamId;

            var expectedMetadata = metadata.Where(r => r.StreamId == streamId).ToList();

            // Act
            var predicate = predicateProvider.StreamMetadata(streamId);

            // Assert
            metadata.Where(predicate.Compile())
                .Should().NotBeEmpty()
                .And.BeEquivalentTo(expectedMetadata);
        }


        [Fact]
        public void When_getting_stream_events_predicate()
        {
            // Arrange
            var predicateProvider = new CassandraPredicateProvider();
            var events = Generated.CreateMany<EventEntity>(10);
            var streamId = events.First().StreamId;

            var expectedEvents = events.Where(r => r.StreamId == streamId).ToList();

            // Act
            var predicate = predicateProvider.StreamEvents(streamId);

            // Assert
            events.Where(predicate.Compile())
                .Should().NotBeEmpty()
                .And.BeEquivalentTo(expectedEvents);
        }

        [Fact]
        public void When_getting_stream_events_start_from()
        {
            // Arrange
            var predicateProvider = new CassandraPredicateProvider();
            var events = Generated.CreateMany<EventEntity>(100);
            var streamId = events.First().StreamId;
            var startFrom = events.First().Revision;
            var expectedEvents = events.Where(r => r.StreamId == streamId && r.Revision >= startFrom).ToList();

            // Act
            var predicate = predicateProvider.StreamEvents(streamId, startFrom);

            // Assert
            events.Where(predicate.Compile())
                .Should().NotBeEmpty()
                .And.BeEquivalentTo(expectedEvents);
        }
    }
}
