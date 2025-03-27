using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.InMemory.Tests.StreamDatabase
{
    public class Getting_actual_revision : Get_actual_revision<InMemoryTestEnvironment>
    {
        public Getting_actual_revision() : base(new InMemoryTestEnvironment())
        {
        }
    }
    public class Reading_from_database : Reading_from_database<InMemoryTestEnvironment>
    {
        public Reading_from_database(ITestOutputHelper output) : base(new InMemoryTestEnvironment(), output)
        {
        }
    }

    public class Deleting_from_database : Deleting_from_database<InMemoryTestEnvironment>
    {
        public Deleting_from_database() : base(new InMemoryTestEnvironment())
        {
        }
    }

    public class Writing_to_database : Writing_to_database<InMemoryTestEnvironment>
    {
        public Writing_to_database() : base(new InMemoryTestEnvironment())
        {
        }
    }
}
