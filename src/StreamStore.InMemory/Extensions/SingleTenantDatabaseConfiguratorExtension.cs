namespace StreamStore.InMemory.Extensions
{
    public static class SingleTenantDatabaseConfiguratorExtension
    {
        public static ISingleTenantDatabaseConfigurator UseInMemoryDatabase(this ISingleTenantDatabaseConfigurator registrator)
        {
            return registrator.UseDatabase<InMemoryStreamDatabase>();
        }
    }
}
