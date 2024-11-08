namespace StreamStore.S3.IntegrationTests.Old.B2
{
    public class B2S3ClientIntegrationTests : S3ClientIntegrationTestsBase<B2S3TestsSuite>
    {
        public B2S3ClientIntegrationTests() : base(new B2S3TestsSuite())
        {
        }

    }
}
