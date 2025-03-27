using StreamStore.Testing.Framework;

namespace StreamStore.Testing.StreamDatabase
{
    public class DatabaseFixtureTestEnvironmentBase: DatabaseTestEnvironmentBase
    {
        readonly IDatabaseFixture fixture;

        protected DatabaseFixtureTestEnvironmentBase(IDatabaseFixture fixture): base()
        {
            this.fixture = fixture.ThrowIfNull(nameof(fixture));
        }

        public override MemoryDatabase Container => fixture.Container;

        protected override void ConfigureDatabase(ISingleTenantConfigurator configurator)
        {
           fixture.ConfigureDatabase(configurator);
        }
        protected override void SetUpInternal()
        {
        }

        protected override bool CheckIfReady()
        {
            return fixture.IsDatabaseReady;
        }
    }
}
