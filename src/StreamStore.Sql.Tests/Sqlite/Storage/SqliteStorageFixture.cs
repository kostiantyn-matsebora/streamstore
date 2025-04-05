﻿using StreamStore.Sql.Sqlite;
using StreamStore.Sql.Tests.Storage;
using StreamStore.Testing;


namespace StreamStore.Sql.Tests.Sqlite.Storage
{
    public sealed class SqliteStorageFixture : SqlStorageFixtureBase<SqliteTestStorage>
    {
        public SqliteStorageFixture(): base(new SqliteTestStorage($"{Generated.Names.Storage}.sqlite"))
        {
        }

        public override void ConfigureStorage(ISingleTenantConfigurator configurator)
        {
               configurator.UseSqliteStorage(
                    c => c.ConfigureStorage(
                        x => x.WithConnectionString(testStorage.ConnectionString)));
        }
    }
}
