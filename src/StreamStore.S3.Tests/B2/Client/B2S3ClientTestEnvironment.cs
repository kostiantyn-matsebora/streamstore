using System.Net;
using Bytewizer.Backblaze.Client;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.S3.B2;
using StreamStore.Testing;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.B2.Client
{
    public class B2S3ClientTestEnvironment: TestEnvironmentBase
    {

        public MockRepository MockRepository { get; }

        public Mock<IStorageClient> B2Client { get; }

        public B2StreamStorageSettings Settings { get; }


        internal B2S3Client CreateB2S3Client()
        {
            return new B2S3Client(Settings,B2Client.Object);
        }

        public B2S3ClientTestEnvironment()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);

            B2Client = MockRepository.Create<IStorageClient>();

            var configurator =
                new B2StorageConfigurator();

            Settings = configurator
                .WithBucketId(Generated.Primitives.String)
                .WithBucketName(Generated.Primitives.String)
                .WithCredential(Generated.Primitives.String, Generated.Primitives.String)
                .Build();
        }
    }
}
