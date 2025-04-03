namespace StreamStore.InMemory.Extensions
{
    public static class SingleTenantStorageConfiguratorExtension
    {
        public static ISingleTenantConfigurator UseInMemoryStorage(this ISingleTenantConfigurator registrator)
        {
            return registrator.UseStorage<InMemoryStreamStorage>();
        }
    }
}
