using Microsoft.Extensions.Configuration;
using StreamStore.Sql.Sqlite;
using StreamStore.Testing;
using StreamStore.Testing.StreamDatabase;


namespace StreamStore.Sql.Tests.Sqlite.Database
{
    public class SqliteTestSuite : DatabaseSuiteBase
    {
        readonly SqliteDatabaseFixture fixture;

        public SqliteTestSuite(): this(new SqliteDatabaseFixture())
        {
        }

        public SqliteTestSuite(SqliteDatabaseFixture fixture)
        {
            this.fixture = fixture.ThrowIfNull(nameof(fixture));
        }

        public override MemoryDatabase Container => fixture.Container;

        protected override void ConfigureDatabase(IStreamStoreConfigurator configurator)
        {
            configurator.UseSqliteDatabase(CreateConfiguration());
        }

        IConfiguration CreateConfiguration()
        {
            return ConfigureConfiguration(new ConfigurationBuilder()).Build();
        }

        IConfigurationBuilder ConfigureConfiguration(IConfigurationBuilder builder)
        {
            return builder.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { $"connectionStrings:StreamStore", $"Data Source={fixture.DatabaseName};Version=3;" },
            });
        }

        protected override void SetUp()
        {
        }
    }
}
