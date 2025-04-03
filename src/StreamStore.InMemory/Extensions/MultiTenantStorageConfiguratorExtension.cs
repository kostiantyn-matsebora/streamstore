namespace StreamStore.InMemory.Extensions
{
    public static class MultiTenantStorageConfiguratorExtension
    {
        public static IMultitenancyConfigurator UseInMemoryStorage(this IMultitenancyConfigurator registrator)
        {
            return registrator.UseStorageProvider<InMemoryStreamStorageProvider>();
        }
    }
}
