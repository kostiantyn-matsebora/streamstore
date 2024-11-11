﻿using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Sql.Tests.PostgreSql.Database
{
    [Collection("Reading Postgres")]
    public class Finding_stream_metadata : Find_stream_data<PostgresTestSuite>
    {
        public Finding_stream_metadata(PostgresDatabaseFixture fixture) : base(new PostgresTestSuite(fixture))
        {
        }
    }

    [Collection("Reading Postgres")]

    public class Reading_from_database : Reading_from_database<PostgresTestSuite>
    {
        public Reading_from_database(PostgresDatabaseFixture fixture, ITestOutputHelper output) : base(new PostgresTestSuite(fixture), output)
        {
        }
    }


   [Collection("Deleting Postgres")]
    public class Deleting_from_database : Deleting_from_database<PostgresTestSuite>
    {
        public Deleting_from_database(PostgresDatabaseFixture fixture) : base(new PostgresTestSuite(fixture))
        {
        }
    }

   [Collection("Writing Postgres")]
    public class Writing_to_database : Writing_to_database<PostgresTestSuite>
    {
        public Writing_to_database(PostgresDatabaseFixture fixture) : base(new PostgresTestSuite(fixture))
        {
        }
    }
}
