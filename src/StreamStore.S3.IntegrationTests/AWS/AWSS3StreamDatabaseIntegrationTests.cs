using StreamStore.Testing.Framework;

namespace StreamStore.S3.IntegrationTests.AWS
{
    public class AWSS3StreamDatabaseIntegrationTests : StreamDatabaseTestsBase<AWSS3TestsSuite>
    {
        public AWSS3StreamDatabaseIntegrationTests() : base(new AWSS3TestsSuite())
        {
        }
    }
}
