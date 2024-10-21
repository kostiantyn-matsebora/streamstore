using System;
using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.InMemory
{
    public class S3StreamDatabaseInMemoryTests : StreamDatabaseTestsBase<S3InMemorySuite>
    {
        public S3StreamDatabaseInMemoryTests() : base(new S3InMemorySuite())
        {

        }
    }
}
