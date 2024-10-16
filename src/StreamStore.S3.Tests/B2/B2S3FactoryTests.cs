﻿using AutoFixture;
using Bytewizer.Backblaze.Client;
using FluentAssertions;
using Moq;
using StreamStore.S3.B2;
using StreamStore.S3.Concurrency;
using StreamStore.Testing;
using System;
using Xunit;

namespace StreamStore.S3.Tests.B2
{
    public class B2S3FactoryTests
    {
        MockRepository mockRepository;

        B2StreamDatabaseSettings settings;
        Mock<IStorageClientFactory> factoryMock;
        public B2S3FactoryTests()
        {
            mockRepository = new MockRepository(MockBehavior.Strict);
            factoryMock = new Mock<IStorageClientFactory>();
            var fixture = new Fixture();
            settings = new B2StreamDatabaseSettingsBuilder()
                .WithBucketId(fixture.Create<string>())
                .WithBucketName(fixture.Create<string>())
                .WithCredentials(fixture.Create<string>(), fixture.Create<string>())
                .Build();
        }

        [Fact]
        public void CreateClient_Should_CreateClientAndConnect()
        {
            // Arrange

            var client = new Mock<IStorageClient>();
            factoryMock.Setup(m => m.Create()).Returns(client.Object);
            client.Setup(client => client.Connect(settings.Credentials!.AccessKeyId, settings.Credentials!.AccessKey));
            // Act
            var factory = new B2S3Factory(settings, factoryMock.Object);
            var result = factory.CreateClient();

            // Assert
            mockRepository.VerifyAll();
            factoryMock.VerifyAll();
        }

        [Fact]
        public void CreateLock_Should_CreateLock()
        {
            // Arrange
            var client = new Mock<IStorageClient>();
            factoryMock.Setup(m => m.Create()).Returns(client.Object);
            client.Setup(client => client.Connect(settings.Credentials!.AccessKeyId, settings.Credentials!.AccessKey));
            var ctx = new Mock<IS3TransactionContext>();
            ctx.SetupGet(x => x.StreamId).Returns(GeneratedValues.String);

            // Act
            var factory = new B2S3Factory(settings, factoryMock.Object);
            var result = factory.CreateLock(ctx.Object);

            // Assert
            result.Should().NotBeNull();
            factoryMock.VerifyAll();
        }
    }
}
