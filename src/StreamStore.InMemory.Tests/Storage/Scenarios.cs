using StreamStore.Testing.StreamStorage.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.InMemory.Tests.StreamStorage
{
    public class Getting_actual_revision : Get_actual_revision<InMemoryTestEnvironment>
    {
        public Getting_actual_revision() : base(new InMemoryTestEnvironment())
        {
        }
    }
    public class Reading_from_storage : Reading_from_storage<InMemoryTestEnvironment>
    {
        public Reading_from_storage(ITestOutputHelper output) : base(new InMemoryTestEnvironment(), output)
        {
        }
    }

    public class Deleting_from_storage : Deleting_from_storage<InMemoryTestEnvironment>
    {
        public Deleting_from_storage() : base(new InMemoryTestEnvironment())
        {
        }
    }

    public class Writing_to_storage : Writing_to_storage<InMemoryTestEnvironment>
    {
        public Writing_to_storage() : base(new InMemoryTestEnvironment())
        {
        }
    }
}
