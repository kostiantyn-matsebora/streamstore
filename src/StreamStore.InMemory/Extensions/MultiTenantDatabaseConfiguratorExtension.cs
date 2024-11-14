namespace StreamStore.InMemory.Extensions
{
    public static class MultiTenantDatabaseConfiguratorExtension
    {
        public static IMultitenancyConfigurator UseInMemoryDatabase(this IMultitenancyConfigurator registrator)
        {
            return registrator.UseDatabaseProvider<InMemoryStreamDatabaseProvider>();
        }
    }
}
