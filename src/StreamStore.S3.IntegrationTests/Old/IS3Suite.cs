﻿using StreamStore.S3.Client;
using StreamStore.Testing;


namespace StreamStore.S3.IntegrationTests.Old
{
    public interface IS3Suite : ITestSuite
    {
        IS3LockFactory? CreateLockFactory();
        IS3ClientFactory? CreateClientFactory();
    }
}