
using Microsoft.Extensions.Configuration;
using StreamStore.Testing;
using StreamStore.Testing.StreamDatabase;

namespace StreamStore.Sql.Tests.Database
{
    public abstract class SqlTestSuiteBase : DatabaseSuiteBase {

        protected readonly SqlDatabaseFixtureBase fixture;

       
        protected SqlTestSuiteBase(SqlDatabaseFixtureBase fixture)
        {
            this.fixture = fixture.ThrowIfNull(nameof(fixture));
        }

        public override MemoryDatabase Container => fixture.Container;

        protected abstract string GetConnectionString();
        protected override void SetUp() { }

        protected IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            {
                { $"connectionStrings:StreamStore", GetConnectionString() },
            }).Build();
        }
    }
}
