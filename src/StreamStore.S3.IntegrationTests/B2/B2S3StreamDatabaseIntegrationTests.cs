using StreamStore.Testing.Framework;

namespace StreamStore.S3.IntegrationTests.B2
{
    public class B2S3StreamDatabaseIntegrationTests : StreamDatabaseTestsBase<B2S3TestsSuite>
    {
        public B2S3StreamDatabaseIntegrationTests() : base(new B2S3TestsSuite())
        {
        }
    }
}
