using StreamStore.S3.Tests.B2;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.AWS
{
    public class AWSS3StreamDatabaseTests : StreamDatabaseTestsBase
    {
        public AWSS3StreamDatabaseTests() : base(new AWSS3TestsSuite())
        {
        }
    }
}
