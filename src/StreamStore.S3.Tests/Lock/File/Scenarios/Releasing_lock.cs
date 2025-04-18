﻿using StreamStore.Testing;

namespace StreamStore.S3.Tests.Lock.File.Scenarios
{
    public class Releasing_lock : Scenario<S3FileLockTestEnvironment>
    {
        public Releasing_lock() : base(new S3FileLockTestEnvironment())
        {
        }

        [SkippableFact]
        public async Task When_release_is_successfull()
        {
            TrySkip();

            // Arrange
            CancellationToken token = default;
            var handle = Environment.CreateS3FileLockHandle();


            // Act
            await handle.ReleaseAsync(token);
            await handle.ReleaseAsync(token); //Checking that the lock is only released once

            // Assert
            Environment.MockRepository.VerifyAll();
        }

        [Fact]
        public async Task When_dispose_is_deleting_lock()
        {
            TrySkip();

            // Arrange
            var handle = Environment.CreateS3FileLockHandle();

            // Act
            await handle.DisposeAsync();
#pragma warning disable S3966 // Objects should not be disposed more than once
            await handle.DisposeAsync(); //Checking that the lock is only released once
#pragma warning restore S3966 // Objects should not be disposed more than once

            // Assert
            Environment.MockRepository.VerifyAll();
        }
    }
}
