﻿using StreamStore.Testing;

namespace StreamStore.S3.IntegrationTests.AWS
{
    public class AWSS3StreamDatabaseIntegrationTests : DatabaseIntegrationTestsBase
    {
        public AWSS3StreamDatabaseIntegrationTests() : base(new AWSS3TestsSuite())
        {
        }
    }
}