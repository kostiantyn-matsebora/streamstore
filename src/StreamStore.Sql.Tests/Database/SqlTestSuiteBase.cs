using StreamStore.Testing;
using StreamStore.Testing.StreamDatabase;

namespace StreamStore.Sql.Tests.Database
{
    public abstract class SqlTestSuiteBase<TDatabase> : DatabaseSuiteBase where TDatabase: ISqlTestDatabase {

        protected readonly SqlDatabaseFixtureBase<TDatabase> fixture;

       
        protected SqlTestSuiteBase(SqlDatabaseFixtureBase<TDatabase> fixture)
        {
            this.fixture = fixture.ThrowIfNull(nameof(fixture));
        }

        public override MemoryDatabase Container => fixture.Container;
       
        protected override void SetUp() { }

        protected override bool CheckPrerequisities()
        {
            return fixture.IsDatabaseReady;
        }
        protected override void ConfigureDatabase(ISingleTenantConfigurator configurator)
        {
            fixture.ConfigureDatabase(configurator);
        }
    }
}
