using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;


namespace StreamStore.S3.Tests.Lock.Composite.LockHandle
{
    public class S3CompositeLockTestEnvironment: LockTestEnvironmentBase
    {
        public Mock<IS3StreamLock> Lock1 { get; }
        public Mock<IS3StreamLock> Lock2 { get; }

        public S3CompositeLockTestEnvironment(): base()
        {
            Lock1 = MockRepository.Create<IS3StreamLock>();
            Lock2 = MockRepository.Create<IS3StreamLock>();
        }

        internal S3CompositeStreamLock CreateLock()
        {
            return new S3CompositeStreamLock(Lock1.Object, Lock2.Object);
        }
    }
}
