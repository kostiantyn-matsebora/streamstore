using Moq;

using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.Lock
{
    public class LockTestSuiteBase: TestSuiteBase
    {

        public MockRepository MockRepository { get; }

        public LockTestSuiteBase()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
        }
    }
}
