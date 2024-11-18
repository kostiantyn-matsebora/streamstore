using StreamStore.Testing.Framework;


namespace StreamStore.Sql.Tests.Database
{
    public abstract class SqlDatabaseFixtureBase<TDatabase>: DatabaseFixtureBase<TDatabase> where TDatabase: ISqlTestDatabase
    {
        protected SqlDatabaseFixtureBase(TDatabase database): base(database)
        {
        }
    }
}
