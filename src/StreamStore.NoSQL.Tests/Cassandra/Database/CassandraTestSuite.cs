using StreamStore.Testing.StreamDatabase;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    public class CassandraTestSuite : DatabaseFixtureSuiteBase
    {

        public CassandraTestSuite() : this(new CassandraDatabaseFixture())
        {
        }

        public CassandraTestSuite(CassandraDatabaseFixture fixture): base(fixture)
        {
        }


        protected override void SetUp()
        {
        }
    }
}
