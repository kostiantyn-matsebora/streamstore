using System.Net;
using AutoFixture;
using Moq;
using StreamStore.S3.B2;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.B2.Factory
{
    public class B2S3FactoryTestEnvironment : TestEnvironmentBase
    {
        public MockRepository MockRepository { get;  }
        public B2StreamDatabaseSettings Settings { get; }
        public Mock<IStorageClientFactory> FactoryMock { get; }

        public readonly Id StreamId = Generated.Primitives.Id;
        
        public readonly Id TransactionId = Generated.Primitives.Id;

        public B2S3FactoryTestEnvironment()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
            FactoryMock = new Mock<IStorageClientFactory>();
            var fixture = new Fixture();
            Settings = new B2StreamDatabaseSettingsBuilder()
                .WithBucketId(fixture.Create<string>())
                .WithBucketName(fixture.Create<string>())
                .WithCredential(fixture.Create<string>(), fixture.Create<string>())
                .Build();
        }
    }
}
