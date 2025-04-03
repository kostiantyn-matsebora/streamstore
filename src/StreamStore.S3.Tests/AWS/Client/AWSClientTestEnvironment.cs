using Amazon.S3;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using StreamStore.S3.AWS;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.AWS.Client
{
    public class AWSS3ClientTestEnvironment: TestEnvironmentBase
    {

        public MockRepository MockRepository { get; private set; }

        public Mock<IAmazonS3> AmazonClient { get; private set; }

        internal AWSS3Client Client { get; private set; }

        public AWSS3StorageSettings Settings { get; private set; }

        public AWSS3ClientTestEnvironment()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);

            AmazonClient = MockRepository.Create<IAmazonS3>();
            var configurator =
                new AWSS3StorageConfigurator(new ServiceCollection());

           Settings = configurator.Build();
           Client = new AWSS3Client(
                    AmazonClient.Object,
                    Settings);
        }
    }
}
