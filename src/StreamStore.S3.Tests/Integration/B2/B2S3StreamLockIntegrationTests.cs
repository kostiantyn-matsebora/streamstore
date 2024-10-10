namespace StreamStore.S3.Tests.Integration.B2
{
    public class B2S3StreamLockIntegrationTests : S3StreamLockIntegrationTests
    {
        public B2S3StreamLockIntegrationTests() : base(B2S3TestsSuite.CreateFactory())
        {
        }
    }
}
