using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public class Finding_stream_metadata : Find_stream_data<SqliteTestSuite>
    {
        public Finding_stream_metadata() : base(new SqliteTestSuite())
        {
        }
    }
    public class Reading_from_database : Reading_from_database<SqliteTestSuite>
    {
        public Reading_from_database(ITestOutputHelper output) : base(new SqliteTestSuite(), output)
        {
        }
    }

    public class Deleting_from_database : Deleting_from_database<SqliteTestSuite>
    {
        public Deleting_from_database() : base(new SqliteTestSuite())
        {
        }
    }

    public class Writing_to_database : Writing_to_database<SqliteTestSuite>
    {
        public Writing_to_database() : base(new SqliteTestSuite())
        {
        }
    }
}
