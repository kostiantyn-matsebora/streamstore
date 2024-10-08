using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2
{
    public class B2S3StreamUowTests : StreamUnitOfWorkTestsBase
    {
        public B2S3StreamUowTests() : base(new B2S3TestsSuite())
        {
        }
    }
}
