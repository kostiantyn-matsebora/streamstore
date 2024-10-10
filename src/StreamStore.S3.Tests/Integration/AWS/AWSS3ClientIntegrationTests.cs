namespace StreamStore.S3.Tests.Integration.AWS
{
    public class AWSS3ClientIntegrationTests : S3ClientIntegrationTestsBase
    {
        public AWSS3ClientIntegrationTests() : base(AWSS3TestsSuite.CreateFactory())
        {
        }

    }
}
