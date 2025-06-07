using Microsoft.Extensions.DependencyInjection;
using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Storage;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.Sqlite.Storage
{
    public sealed class SqliteStorageFixture : SqlStorageFixtureBase<SqliteTestStorage>
    {
        public SqliteStorageFixture() : base(new SqliteTestStorage($"{Generated.Names.Storage}.sqlite"))
        {
        }

        public override void ConfigurePersistence(IServiceCollection services)
        {
            services.AddSqlite(c => c.WithConnectionString(testStorage.ConnectionString));
        }
    }
}
