﻿using StreamStore.Testing;

namespace StreamStore.S3.Tests.Integration.B2
{
    public class B2S3StreamUowIntegrationTests : StreamUnitOfWorkTestsBase
    {
        public B2S3StreamUowIntegrationTests() : base(new B2S3TestsSuite())
        {
        }
    }
}
