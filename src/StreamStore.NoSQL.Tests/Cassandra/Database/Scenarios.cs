﻿
using StreamStore.Testing.StreamDatabase.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.NoSql.Tests.Cassandra.Database
{
    [Collection("Reading Cassandra")]
    public class Finding_stream_metadata : Find_stream_data<CassandraTestSuite>
    {
        public Finding_stream_metadata(CassandraDatabaseFixture fixture) : base(new CassandraTestSuite(fixture))
        {
        }
    }

    [Collection("Reading Cassandra")]

    public class Reading_from_database : Reading_from_database<CassandraTestSuite>
    {
        public Reading_from_database(CassandraDatabaseFixture fixture, ITestOutputHelper output) : base(new CassandraTestSuite(fixture), output)
        {
        }
    }


    [Collection("Deleting Cassandra")]
    public class Deleting_from_database : Deleting_from_database<CassandraTestSuite>
    {
        public Deleting_from_database(CassandraDatabaseFixture fixture) : base(new CassandraTestSuite(fixture))
        {
        }
    }

    [Collection("Writing Cassandra")]
    public class Writing_to_database : Writing_to_database<CassandraTestSuite>
    {
        public Writing_to_database(CassandraDatabaseFixture fixture) : base(new CassandraTestSuite(fixture))
        {
        }
    }
}
