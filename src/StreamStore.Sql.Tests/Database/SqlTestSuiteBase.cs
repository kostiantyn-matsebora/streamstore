using StreamStore.Testing.StreamDatabase;

namespace StreamStore.Sql.Tests.Database
{
    public abstract class SqlTestSuiteBase<TDatabase> : DatabaseFixtureSuiteBase where TDatabase: ISqlTestDatabase {


        protected SqlTestSuiteBase(SqlDatabaseFixtureBase<TDatabase> fixture): base(fixture)
        {
        }
    }
}
