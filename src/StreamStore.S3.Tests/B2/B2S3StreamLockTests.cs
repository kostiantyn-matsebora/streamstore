
namespace StreamStore.S3.Tests.B2
{
    public class B2S3StreamLockTests: S3StreamLockTests
    {
        public B2S3StreamLockTests(): base(B2TestsSuite.CreateFactory())
        {
        }
    }
}
