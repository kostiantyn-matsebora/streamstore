﻿using StreamStore.Testing;

namespace StreamStore.S3.Tests.Integration.B2
{
    public class B2S3StreamDatabaseIntegrationTests : StreamDatabaseTestsBase
    {
        public B2S3StreamDatabaseIntegrationTests() : base(new B2S3TestsSuite())
        {
        }
    }
}
