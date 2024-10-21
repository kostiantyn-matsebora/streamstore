using StreamStore.Testing.Framework;

namespace StreamStore.S3.IntegrationTests.AWS
{
    public class AWSS3StreamUowIntegrationTests : StreamUnitOfWorkTestsBase<AWSS3TestsSuite>
    {
        public AWSS3StreamUowIntegrationTests() : base(new AWSS3TestsSuite())
        {
        }
    }
}
