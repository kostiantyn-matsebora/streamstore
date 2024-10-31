using Moq;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;


namespace StreamStore.S3.Tests.Lock.Composite.LockHandle
{
    public class S3CompositeLockTestSuite: LockTestSuiteBase
    {
        public Mock<IS3LockHandle> FirstHandle { get; }
        public Mock<IS3LockHandle> SecondHandle { get; }

        public Mock<IS3StreamLock> Lock1 { get; }
        public Mock<IS3StreamLock> Lock2 { get; }

        public S3CompositeLockTestSuite(): base()
        {
            FirstHandle = MockRepository.Create<IS3LockHandle>();
            SecondHandle = MockRepository.Create<IS3LockHandle>();
            Lock1 = MockRepository.Create<IS3StreamLock>();
            Lock2 = MockRepository.Create<IS3StreamLock>();
        }

        internal S3CompositeLockHandle CreateHandle()
        {
            return new S3CompositeLockHandle(FirstHandle.Object, SecondHandle.Object);
        }

        internal S3CompositeStreamLock CreateLock()
        {
            return new S3CompositeStreamLock(Lock1.Object, Lock2.Object);
        }
    }
}
