using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    [Collection("Reading Cassandra")]
    public class Getting_actual_revision : Get_actual_revision<CassandraDatabaseTestSuite>
    {
        public Getting_actual_revision(CassandraDatabaseFixture fixture) : base(new CassandraDatabaseTestSuite(fixture))
        {
        }
    }

    [Collection("Reading Cassandra")]

    public class Reading_from_database : Reading_from_database<CassandraDatabaseTestSuite>
    {
        public Reading_from_database(CassandraDatabaseFixture fixture, ITestOutputHelper output) : base(new CassandraDatabaseTestSuite(fixture), output)
        {
        }
    }


    [Collection("Deleting Cassandra")]
    public class Deleting_from_database : Deleting_from_database<CassandraDatabaseTestSuite>
    {
        public Deleting_from_database(CassandraDatabaseFixture fixture) : base(new CassandraDatabaseTestSuite(fixture))
        {
        }
    }

    [Collection("Writing Cassandra")]
    public class Writing_to_database : Writing_to_database<CassandraDatabaseTestSuite>
    {
        public Writing_to_database(CassandraDatabaseFixture fixture) : base(new CassandraDatabaseTestSuite(fixture))
        {
        }
    }
}
