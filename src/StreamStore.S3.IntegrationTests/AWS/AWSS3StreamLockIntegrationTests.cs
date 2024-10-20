namespace StreamStore.S3.IntegrationTests.AWS
{
    public class AWSS3StreamLockIntegrationTests : S3StreamLockIntegrationTests
    {
        public AWSS3StreamLockIntegrationTests() : base(new AWSS3TestsSuite())
        {
        }
    }
}
