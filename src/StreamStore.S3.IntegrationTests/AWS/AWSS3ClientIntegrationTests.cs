namespace StreamStore.S3.IntegrationTests.AWS
{
    public class AWSS3ClientIntegrationTests : S3ClientIntegrationTestsBase<AWSS3TestsSuite>
    {
        public AWSS3ClientIntegrationTests() : base(new AWSS3TestsSuite())
        {
        }

    }
}
