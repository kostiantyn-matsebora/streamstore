
using Microsoft.Extensions.DependencyInjection;

namespace StreamStore.InMemory
{
    public static class StreamStoreConfiguratorExtension
    {
        public static ISingleTenantDatabaseConfigurator UseInMemoryDatabase(this ISingleTenantDatabaseConfigurator registrator)
        {
            return registrator.UseDatabase<InMemoryStreamDatabase>();
        }
    }
}
