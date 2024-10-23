using System.Collections.Concurrent;
using AutoFixture;
using Bytewizer.Backblaze.Extensions;
using FluentAssertions;
using StreamStore.S3.Client;
using StreamStore.Testing;
using StreamStore.Testing.Framework;


namespace StreamStore.S3.IntegrationTests
{
    public abstract class S3StreamLockIntegrationTests<TSuite>: Scenario<TSuite> where TSuite: IS3Suite
    {
        private readonly IS3LockFactory? lockFactory;

        protected S3StreamLockIntegrationTests(TSuite suite): base(suite)
        {
            this.lockFactory = suite.ArePrerequisitiesMet? suite.CreateLockFactory() : null!;
        }

        [InlineData(1000)]
        [InlineData(100)]
        [InlineData(10)]
        [InlineData(1)]
        [SkippableTheory]
        public async Task AcquireAsync_ShouldAcquireLockOnlyOnce(int parallelAttempts)
        {
            TrySkip();

            // Arrange
            var fixture = new Fixture();

            var acquirances = new ConcurrentBag<IS3LockHandle>();
            var streamId = Generated.Id;
            var transactionId = Generated.Id;

            // Act
            await Enumerable.Range(0, parallelAttempts).ForEachAsync(parallelAttempts, async i =>
            {
                var @lock = lockFactory!.CreateLock(streamId, transactionId);
                var handle = await @lock.AcquireAsync(CancellationToken.None);

                if (handle != null)
                    acquirances.Add(handle);
            });

            // Assert
            acquirances.Count.Should().Be(1);

            var releaseTasks = acquirances.Select(handle => handle.ReleaseAsync(CancellationToken.None));
            await Task.WhenAll(releaseTasks);
        }


        [SkippableFact]
        public async Task AcquireAsync_ShouldNotAllowToAcquireLockIfAlreadyExists()
        {
            TrySkip();

            // Arrange
            var fixture = new Fixture();
            var streamId = Generated.Id;
            var transactionId = Generated.Id;

            // Act & Assert
            var @lock = lockFactory!.CreateLock(streamId, transactionId);
            var handle = await @lock.AcquireAsync(CancellationToken.None);
            handle.Should().NotBeNull();

            var lock2 = lockFactory.CreateLock(streamId, transactionId);
            var handle2 = await lock2.AcquireAsync(CancellationToken.None);
            handle2.Should().BeNull();

            await handle!.ReleaseAsync(CancellationToken.None);
        }

        [SkippableFact]
        public async Task AcquireAsync_ShouldAcquireLockIfReleased()
        {
            TrySkip();

            // Arrange
            var fixture = new Fixture();
            var streamId = Generated.Id;
            var transactionId = Generated.Id;

            // Act & Assert
            var @lock = lockFactory!.CreateLock(streamId, transactionId);
            var handle = await @lock.AcquireAsync(CancellationToken.None);
            handle.Should().NotBeNull();

            (await @lock.AcquireAsync(CancellationToken.None)).Should().BeNull();

            await handle!.ReleaseAsync(CancellationToken.None);

            var lock2 = lockFactory.CreateLock(streamId, transactionId);
            var handle2 = await lock2.AcquireAsync(CancellationToken.None);
            handle2.Should().NotBeNull();

            await handle2!.ReleaseAsync(CancellationToken.None);
        }
    }
}
