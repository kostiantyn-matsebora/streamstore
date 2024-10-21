using StreamStore.Testing.Framework;

namespace StreamStore.S3.Tests.InMemory
{
    public class S3StreamUowInMemoryTests : StreamUnitOfWorkTestsBase<S3InMemorySuite>
    {
        public S3StreamUowInMemoryTests() : base(new S3InMemorySuite())
        {
        }
    }
}
