using StreamStore.Testing.StreamStorage.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.NoSql.Tests.Cassandra.CosmosDb.Storage
{
    [Collection("Reading Cassandra CosmosDb")]
    public class Getting_stream_metadata : Getting_stream_metadata<CosmosDbStorageTestEnvironment>
    {
        public Getting_stream_metadata(CosmosDbStorageFixture fixture) : base(new CosmosDbStorageTestEnvironment(fixture))
        {
        }
    }

    [Collection("Reading Cassandra CosmosDb")]

    public class Reading_from_storage : Reading_from_storage<CosmosDbStorageTestEnvironment>
    {
        public Reading_from_storage(CosmosDbStorageFixture fixture, ITestOutputHelper output) : base(new CosmosDbStorageTestEnvironment(fixture), output)
        {
        }
    }


    [Collection("Deleting Cassandra CosmosDb")]
    public class Deleting_from_storage : Deleting_from_storage<CosmosDbStorageTestEnvironment>
    {
        public Deleting_from_storage(CosmosDbStorageFixture fixture) : base(new CosmosDbStorageTestEnvironment(fixture))
        {
        }
    }

    [Collection("Writing Cassandra CosmosDb")]
    public class Writing_to_storage : Writing_to_storage<CosmosDbStorageTestEnvironment>
    {
        public Writing_to_storage(CosmosDbStorageFixture fixture) : base(new CosmosDbStorageTestEnvironment(fixture))
        {
            SkipEventIdUniquenessCheck = true;
        }
    }
}
