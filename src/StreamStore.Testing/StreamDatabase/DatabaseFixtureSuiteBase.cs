using StreamStore.Testing.Framework;

namespace StreamStore.Testing.StreamDatabase
{
    public class DatabaseFixtureSuiteBase: DatabaseSuiteBase
    {
        readonly IDatabaseFixture fixture;

        protected DatabaseFixtureSuiteBase(IDatabaseFixture fixture): base()
        {
            this.fixture = fixture.ThrowIfNull(nameof(fixture));
        }

        public override MemoryDatabase Container => fixture.Container;

        protected override void ConfigureDatabase(ISingleTenantConfigurator configurator)
        {
           fixture.ConfigureDatabase(configurator);
        }
        protected override void SetUp()
        {
        }

        protected override bool CheckPrerequisities()
        {
            return fixture.IsDatabaseReady;
        }
    }
}
