using System;
using StreamStore.Testing;

namespace StreamStore.S3.Tests.InMemory
{
    public class S3StreamDatabaseInMemoryTests : StreamDatabaseTestsBase
    {
        public S3StreamDatabaseInMemoryTests() : base(new S3InMemorySuite())
        {

        }
    }
}
