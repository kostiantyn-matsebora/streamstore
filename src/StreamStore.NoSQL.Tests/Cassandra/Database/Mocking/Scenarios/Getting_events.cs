﻿
using Cassandra.Mapping;
using FluentAssertions;
using Moq;
using StreamStore.NoSql.Cassandra.Models;
using StreamStore.Testing;

namespace StreamStore.NoSql.Tests.Cassandra.Database.Mocking
{
    public class Getting_events : Scenario<CassandraMockTestSuite>
    {

        [Fact]
        public async Task When_stream_metadata_is_not_found()
        {
            // Arrange
            var streamId = Generated.Id;
            Suite.Mapper.Setup(x => x.FetchAsync<EventMetadataEntity>(It.IsAny<Cql>())).ReturnsAsync(Array.Empty<EventMetadataEntity>());

            // Act
            var result = await Suite.StreamDatabase.FindMetadataAsync(streamId);

            // Assert
            Suite.MockRepository.VerifyAll();
            result.Should().BeNull();
        }

        [Fact]
        public async Task When_stream_metadata_is_found()
        {
            // Arrange
            var streamId = Generated.Id;
            var events = Generated.CreateMany<EventMetadataEntity>(10);

            Suite.Mapper.Setup(x => x.FetchAsync<EventMetadataEntity>(It.IsAny<Cql>())).ReturnsAsync(events);

            // Act
            var result = await Suite.StreamDatabase.FindMetadataAsync(streamId);

            // Assert
            Suite.MockRepository.VerifyAll();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(events.Length);
        }

        [Fact]
        public async Task When_getting_events()
        {
            // Arrange
            var streamId = Generated.Id;
            var events = Generated.CreateMany<EventEntity>(5);
            var startFrom = Generated.Revision;
            var count = 5;

            Suite.Mapper.Setup(x => x.FetchAsync<EventEntity>(It.IsAny<Cql>())).ReturnsAsync(events);
            Suite.Mapper.Setup(x => x.SingleAsync<int?>(It.IsAny<Cql>())).ReturnsAsync(15);
            // Act
            var result = await Suite.StreamDatabase.ReadAsync(streamId, startFrom, count, CancellationToken.None);

            // Assert
            Suite.MockRepository.VerifyAll();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(events.Length);
        }
    }
}
