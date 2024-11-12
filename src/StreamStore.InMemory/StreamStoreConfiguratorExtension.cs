
namespace StreamStore.InMemory
{
    public static class StreamStoreConfiguratorExtension
    {
        public static ISingleTenantDatabaseRegistrator UseInMemoryDatabase(this ISingleTenantDatabaseRegistrator registrator)
        {
            return registrator.RegisterDatabase<InMemoryStreamDatabase>();
        }
    }
}
