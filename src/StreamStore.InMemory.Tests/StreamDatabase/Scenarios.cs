using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.InMemory.Tests.StreamDatabase
{
    public class Getting_actual_revision : Get_actual_revision<InMemoryTestSuite>
    {
        public Getting_actual_revision() : base(new InMemoryTestSuite())
        {
        }
    }
    public class Reading_from_database : Reading_from_database<InMemoryTestSuite>
    {
        public Reading_from_database(ITestOutputHelper output) : base(new InMemoryTestSuite(), output)
        {
        }
    }

    public class Deleting_from_database : Deleting_from_database<InMemoryTestSuite>
    {
        public Deleting_from_database() : base(new InMemoryTestSuite())
        {
        }
    }

    public class Writing_to_database : Writing_to_database<InMemoryTestSuite>
    {
        public Writing_to_database() : base(new InMemoryTestSuite())
        {
        }
    }
}
