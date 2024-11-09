﻿using AutoFixture;
using FluentAssertions;
using Moq;
using StreamStore.S3.Client;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Storage.EventStorage
{
    public class Appending_events: Scenario<S3StorageTestSuite>
    {

        [Fact]
        public async Task When_appending_events()
        {
            // Arrange
            var s3EventStorage = Suite.CreateS3EventStorage();
            var fixture = new Fixture();
            EventRecord record = fixture.Create<EventRecord>();
            CancellationToken token = default;
            Suite.MockS3Client.Setup(x => x.DisposeAsync()).Returns(default(ValueTask));

            Suite.MockS3Client
                .Setup(x => x.UploadObjectAsync(It.Is<UploadObjectRequest>(r => r.Key!.Contains(record.Id) && r.Key.Contains(Suite.Path)), token))
                .ReturnsAsync(fixture.Create<UploadObjectResponse>());

            // Act
            await s3EventStorage.AppendAsync(record, token);

            // Assert
            s3EventStorage.NotEmpty.Should().BeTrue();
            s3EventStorage.Count().Should().Be(1);
            s3EventStorage.First().Event.Should().Be(record);
            s3EventStorage.Last().Event.Should().Be(record);

            Suite.MockRepository.VerifyAll();
        }
    }
}