using StreamStore.S3.Tests.AWS;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.Integration.AWS
{
    public class AWSS3StreamUowIntegrationTests : StreamUnitOfWorkTestsBase
    {
        public AWSS3StreamUowIntegrationTests() : base(new AWSS3TestsSuite())
        {
        }
    }
}
