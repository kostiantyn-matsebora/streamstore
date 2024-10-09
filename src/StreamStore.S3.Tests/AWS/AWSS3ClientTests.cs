using StreamStore.S3.Tests.B2;

namespace StreamStore.S3.Tests.AWS
{
    public class AWSS3ClientTests : S3ClientTestsBase
    {
        public AWSS3ClientTests() : base(AWSS3TestsSuite.CreateFactory())
        {
        }

    }
}
