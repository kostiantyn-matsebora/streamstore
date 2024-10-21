using StreamStore.Testing.Framework;

namespace StreamStore.S3.IntegrationTests.B2
{
    public class B2S3StreamUowIntegrationTests : StreamUnitOfWorkTestsBase<B2S3TestsSuite>
    {
        public B2S3StreamUowIntegrationTests() : base(new B2S3TestsSuite())
        {
        }
    }
}
