using StreamStore.Testing.Framework;


namespace StreamStore.Sql.Tests.Storage
{
    public abstract class SqlStorageFixtureBase<TStorage>: StorageFixtureBase<TStorage> where TStorage: ISqlTestStorage
    {
        protected SqlStorageFixtureBase(TStorage storage): base(storage)
        {
        }
    }
}
