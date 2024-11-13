namespace StreamStore.InMemory.Extensions
{
    public static class MultiTenantDatabaseConfiguratorExtension
    {
        public static IMultitenantDatabaseConfigurator UseInMemoryDatabase(this IMultitenantDatabaseConfigurator registrator)
        {
            return registrator.UseDatabaseProvider<InMemoryStreamDatabaseProvider>();
        }
    }
}
