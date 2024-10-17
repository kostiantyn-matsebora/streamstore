//using System.Collections.Concurrent;
//using AutoFixture;
//using Bytewizer.Backblaze.Extensions;
//using FluentAssertions;
//using StreamStore.S3.Client;
//using StreamStore.S3.Concurrency;


//namespace StreamStore.S3.IntegrationTests
//{
//    public abstract class S3StreamLockIntegrationTests(IS3Factory? factory)
//    {
//        [InlineData(1000)]
//        [InlineData(100)]
//        [InlineData(10)]
//        [InlineData(1)]
//        [SkippableTheory]
//        public async Task AcquireAsync_ShouldAcquireLockOnlyOnce(int parallelAttempts)
//        {
//            Skip.IfNot(factory != null, "Configuration is missing");

//            // Arrange
//            var fixture = new Fixture();
//            var ctx = S3TransactionContext.New(fixture.Create<string>());

//            var acquirances = new ConcurrentBag<IS3LockHandle>();
//            // Act
//            await Enumerable.Range(0, parallelAttempts).ForEachAsync(parallelAttempts, async i =>
//            {
//                var @lock = factory!.CreateLock(ctx);
//                var handle = await @lock.AcquireAsync(CancellationToken.None);

//                if (handle != null)
//                    acquirances.Add(handle);
//            });

//            // Assert
//            acquirances.Count.Should().Be(1);

//            var releaseTasks = acquirances.Select(handle => handle.ReleaseAsync(CancellationToken.None));
//            await Task.WhenAll(releaseTasks);
//        }


//        [SkippableFact]
//        public async Task AcquireAsync_ShouldNotAllowToAcquireLockIfAlreadyExists()
//        {
//            Skip.IfNot(factory != null, "Configuration is missing");

//            // Arrange
//            var fixture = new Fixture();
//            var ctx = S3TransactionContext.New(fixture.Create<string>());

//            // Act & Assert
//            var @lock = factory!.CreateLock(ctx);
//            var handle = await @lock.AcquireAsync(CancellationToken.None);
//            handle.Should().NotBeNull();

//            var lock2 = factory!.CreateLock(ctx);
//            var handle2 = await lock2.AcquireAsync(CancellationToken.None);
//            handle2.Should().BeNull();

//            await handle!.ReleaseAsync(CancellationToken.None);
//        }

//        [SkippableFact]
//        public async Task AcquireAsync_ShouldAcquireLockIfReleased()
//        {
//            Skip.IfNot(factory != null, "Configuration is missing");

//            // Arrange
//            var fixture = new Fixture();
//            var ctx = S3TransactionContext.New(fixture.Create<string>());

//            // Act & Assert
//            var @lock = factory!.CreateLock(ctx);
//            var handle = await @lock.AcquireAsync(CancellationToken.None);
//            handle.Should().NotBeNull();

//            (await @lock.AcquireAsync(CancellationToken.None)).Should().BeNull();

//            await handle!.ReleaseAsync(CancellationToken.None);

//            var lock2 = factory!.CreateLock(ctx);
//            var handle2 = await lock2.AcquireAsync(CancellationToken.None);
//            handle2.Should().NotBeNull();

//            await handle2!.ReleaseAsync(CancellationToken.None);
//        }
//    }
//}
