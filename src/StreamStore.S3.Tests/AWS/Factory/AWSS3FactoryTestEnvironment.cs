using StreamStore.S3.AWS;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.AWS.Factory
{
    public class AWSS3FactoryTestEnvironment : TestEnvironmentBase
    {
        public AWSS3StorageSettings Settings { get; private set; }


        internal AWSS3Factory CreateFactory(IAmazonS3ClientFactory clientFactory)
        {
            return new AWSS3Factory(Settings, clientFactory);
        }

        public AWSS3FactoryTestEnvironment()
        {
            Settings = new AWSS3StorageSettingsBuilder().Build();
        }
    }
}
