using StreamStore.Testing.StreamDatabase;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    public class CassandraDatabaseTestSuite : DatabaseFixtureSuiteBase
    {

        public CassandraDatabaseTestSuite() : this(new CassandraDatabaseFixture())
        {
        }

        public CassandraDatabaseTestSuite(CassandraDatabaseFixture fixture): base(fixture)
        {
        }


        protected override void SetUp()
        {
        }
    }
}
