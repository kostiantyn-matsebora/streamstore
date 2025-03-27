using StreamStore.Testing.StreamDatabase;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    public class CassandraDatabaseTestEnvironment : DatabaseFixtureTestEnvironmentBase
    {

        public CassandraDatabaseTestEnvironment() : this(new CassandraDatabaseFixture())
        {
        }

        public CassandraDatabaseTestEnvironment(CassandraDatabaseFixture fixture): base(fixture)
        {
        }


        protected override void SetUpInternal()
        {
        }
    }
}
