using StreamStore.Testing.StreamStorage;

namespace StreamStore.Sql.Tests.Storage
{
    public abstract class SqlTestEnvironmentBase<TStorage> : StorageFixtureTestEnvironmentBase where TStorage: ISqlTestStorage {


        protected SqlTestEnvironmentBase(SqlStorageFixtureBase<TStorage> fixture): base(fixture)
        {
        }
    }
}
