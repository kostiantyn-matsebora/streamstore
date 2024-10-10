using StreamStore.Testing;

namespace StreamStore.S3.Tests.Integration.AWS
{
    public class AWSS3StreamDatabaseIntegrationTests : StreamDatabaseTestsBase
    {
        public AWSS3StreamDatabaseIntegrationTests() : base(new AWSS3TestsSuite())
        {
        }
    }
}
