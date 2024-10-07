using StreamStore.Testing;

namespace StreamStore.S3.Tests.B2
{
    public class B2S3StreamDatabaseTests : StreamDatabaseTestsBase
    {
        public B2S3StreamDatabaseTests() : base(new B2S3TestsSuite())
        {
        }
    }
}
