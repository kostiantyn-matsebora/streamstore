using StreamStore.Testing.StreamDatabase;

namespace StreamStore.Sql.Tests.Database
{
    public abstract class SqlTestEnvironmentBase<TDatabase> : DatabaseFixtureTestEnvironmentBase where TDatabase: ISqlTestDatabase {


        protected SqlTestEnvironmentBase(SqlDatabaseFixtureBase<TDatabase> fixture): base(fixture)
        {
        }
    }
}
