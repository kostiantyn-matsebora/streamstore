﻿using StreamStore.Sql.Tests.Database;


namespace StreamStore.Sql.Tests.PostgreSql.Database
{
    public class PostgresTestSuite : SqlTestSuiteBase<PostgresTestDatabase>
    {
        public PostgresTestSuite() : this(new PostgresDatabaseFixture())
        {
        }

        public PostgresTestSuite(PostgresDatabaseFixture fixture) : base(fixture)
        {
        }
    }
}
