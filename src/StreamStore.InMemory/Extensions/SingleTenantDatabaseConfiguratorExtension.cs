namespace StreamStore.InMemory.Extensions
{
    public static class SingleTenantDatabaseConfiguratorExtension
    {
        public static ISingleTenantConfigurator UseInMemoryDatabase(this ISingleTenantConfigurator registrator)
        {
            return registrator.UseDatabase<InMemoryStreamDatabase>();
        }
    }
}
