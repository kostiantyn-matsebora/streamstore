using StreamStore.Testing;

namespace StreamStore.S3.Tests.AWS
{
    public class AWSS3StreamUowTests : StreamUnitOfWorkTestsBase
    {
        public AWSS3StreamUowTests() : base(new AWSS3TestsSuite())
        {
        }
    }
}
