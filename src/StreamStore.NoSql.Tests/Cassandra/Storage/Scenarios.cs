using StreamStore.Testing.StreamStorage.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.NoSql.Tests.Cassandra.Storage
{
    [Collection("Reading Cassandra")]
    public class Getting_actual_revision : Get_actual_revision<CassandraStorageTestEnvironment>
    {
        public Getting_actual_revision(CassandraStorageFixture fixture) : base(new CassandraStorageTestEnvironment(fixture))
        {
        }
    }

    [Collection("Reading Cassandra")]

    public class Reading_from_storage : Reading_from_storage<CassandraStorageTestEnvironment>
    {
        public Reading_from_storage(CassandraStorageFixture fixture, ITestOutputHelper output) : base(new CassandraStorageTestEnvironment(fixture), output)
        {
        }
    }


    [Collection("Deleting Cassandra")]
    public class Deleting_from_storage : Deleting_from_storage<CassandraStorageTestEnvironment>
    {
        public Deleting_from_storage(CassandraStorageFixture fixture) : base(new CassandraStorageTestEnvironment(fixture))
        {
        }
    }

    [Collection("Writing Cassandra")]
    public class Writing_to_storage : Writing_to_storage<CassandraStorageTestEnvironment>
    {
        public Writing_to_storage(CassandraStorageFixture fixture) : base(new CassandraStorageTestEnvironment(fixture))
        {
        }
    }
}
