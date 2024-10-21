namespace StreamStore.S3.IntegrationTests.B2
{
    public class B2S3StreamLockIntegrationTests : S3StreamLockIntegrationTests<B2S3TestsSuite>
    {
        public B2S3StreamLockIntegrationTests() : base(new B2S3TestsSuite())
        {
        }
    }
}
