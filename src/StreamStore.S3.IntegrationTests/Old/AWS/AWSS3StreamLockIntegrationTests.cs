namespace StreamStore.S3.IntegrationTests.Old.AWS
{
    public class AWSS3StreamLockIntegrationTests : S3StreamLockIntegrationTests<AWSS3TestsSuite>
    {
        public AWSS3StreamLockIntegrationTests() : base(new AWSS3TestsSuite())
        {
        }
    }
}
