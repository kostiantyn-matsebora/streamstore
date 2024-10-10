namespace StreamStore.S3.Tests.Integration.AWS
{
    public class AWSS3StreamLockIntegrationTests : S3StreamLockIntegrationTests
    {
        public AWSS3StreamLockIntegrationTests() : base(AWSS3TestsSuite.CreateFactory())
        {
        }
    }
}
