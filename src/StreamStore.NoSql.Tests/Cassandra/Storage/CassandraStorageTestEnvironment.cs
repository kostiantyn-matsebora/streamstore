using StreamStore.Testing.StreamStorage;

namespace StreamStore.NoSql.Tests.Cassandra.Storage
{
    public class CassandraStorageTestEnvironment : StorageFixtureTestEnvironmentBase
    {

        public CassandraStorageTestEnvironment() : this(new CassandraStorageFixture())
        {
        }

        public CassandraStorageTestEnvironment(CassandraStorageFixture fixture) : base(fixture)
        {
        }


        protected override void SetUpInternal()
        {
        }
    }
}
