using StreamStore.Testing.StreamStorage.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.NoSql.Tests.DynamoDb.Storage
{
    [Collection("Reading DynamoDb")]
    public class Getting_stream_metadata : Getting_stream_metadata<DynamoDbTestEnvironment>
    {
        public Getting_stream_metadata(DynamoDbStorageFixture fixture) : base(new DynamoDbTestEnvironment(fixture))
        {
        }
    }

    [Collection("Reading DynamoDb")]

    public class Reading_from_storage : Reading_from_storage<DynamoDbTestEnvironment>
    {
        public Reading_from_storage(DynamoDbStorageFixture fixture, ITestOutputHelper output) : base(new DynamoDbTestEnvironment(fixture), output)
        {
        }
    }


    [Collection("Deleting DynamoDb")]
    public class Deleting_from_storage : Deleting_from_storage<DynamoDbTestEnvironment>
    {
        public Deleting_from_storage(DynamoDbStorageFixture fixture) : base(new DynamoDbTestEnvironment(fixture))
        {
        }
    }

    [Collection("Writing DynamoDb")]
    public class Writing_to_storage : Writing_to_storage<DynamoDbTestEnvironment>
    {
        public Writing_to_storage(DynamoDbStorageFixture fixture) : base(new DynamoDbTestEnvironment(fixture))
        {
            SkipEventIdUniquenessCheck = true;
        }
    }
}
