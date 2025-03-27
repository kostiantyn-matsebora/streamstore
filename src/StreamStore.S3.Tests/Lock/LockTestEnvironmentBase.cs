using Moq;

using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.Lock
{
    public class LockTestEnvironmentBase: TestEnvironmentBase
    {

        public MockRepository MockRepository { get; }

        public LockTestEnvironmentBase()
        {
            MockRepository = new MockRepository(MockBehavior.Strict);
        }
    }
}
