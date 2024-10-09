using StreamStore.S3.Tests.B2;

namespace StreamStore.S3.Tests.AWS
{
    public class AWSS3StreamLockTests : S3StreamLockTests
    {
        public AWSS3StreamLockTests() : base(AWSS3TestsSuite.CreateFactory())
        {
        }
    }
}
