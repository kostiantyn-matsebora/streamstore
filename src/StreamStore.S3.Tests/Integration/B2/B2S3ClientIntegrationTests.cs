namespace StreamStore.S3.Tests.Integration.B2
{
    public class B2S3ClientIntegrationTests : S3ClientIntegrationTestsBase
    {
        public B2S3ClientIntegrationTests() : base(B2S3TestsSuite.CreateFactory())
        {
        }

    }
}
