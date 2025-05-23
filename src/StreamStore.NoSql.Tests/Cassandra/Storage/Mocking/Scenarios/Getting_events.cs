using Cassandra.Mapping;
using FluentAssertions;
using Moq;
using StreamStore.NoSql.Cassandra.Models;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Storage.Mocking
{
    public class Getting_events : Scenario<CassandraMockTestEnvironment>
    {

        [Fact]
        public async Task When_stream_is_not_found()
        {
            // Arrange
            var streamId = Generated.Primitives.Id;
            Environment.Mapper.Setup(x => x.SingleOrDefaultAsync<int?>(It.IsAny<Cql>())).ReturnsAsync((int?)null);
            Environment.Queries.Setup(x => x.StreamActualRevision(It.IsAny<string>())).Returns(new Cql(Generated.Primitives.String));

            // Act
            var result = await Environment.StreamStorage.GetMetadata(streamId);

            // Assert
            Environment.MockRepository.VerifyAll();
            result.Should().BeNull();
        }

        [Fact]
        public async Task When_stream_is_found()
        {
            // Arrange
            var streamId = Generated.Primitives.Id;

            Environment.Mapper.Setup(x => x.SingleOrDefaultAsync<int?>(It.IsAny<Cql>())).ReturnsAsync(10);
            Environment.Queries.Setup(x => x.StreamActualRevision(It.IsAny<string>())).Returns(new Cql(Generated.Primitives.String));

            // Act
            var result = await Environment.StreamStorage.GetMetadata(streamId);

            // Assert
            Environment.MockRepository.VerifyAll();
            result.Should().NotBeNull();
            result!.Id.Should().Be(streamId);
            result!.Revision.Should().Be(10);
        }

        [Fact]
        public async Task When_getting_events()
        {
            // Arrange
            var streamId = Generated.Primitives.Id;
            var events = Generated.Objects.Many<EventEntity>(5);
            var startFrom = Generated.Primitives.Revision;
            var count = 5;

            Environment.Mapper.Setup(x => x.FetchAsync<EventEntity>(It.IsAny<Cql>())).ReturnsAsync(events);
            Environment.Mapper.Setup(x => x.SingleOrDefaultAsync<int?>(It.IsAny<Cql>())).ReturnsAsync(15);
            Environment.Queries.Setup(x => x.StreamActualRevision(It.IsAny<string>())).Returns(new Cql(Generated.Primitives.String));
            Environment.Queries.Setup(x => x.StreamEvents(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).Returns(new Cql(Generated.Primitives.String));

            // Act
            var result = await Environment.StreamStorage.ReadAsync(streamId, startFrom, count, CancellationToken.None);

            // Assert
            Environment.MockRepository.VerifyAll();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(events.Length);
        }
    }
}
