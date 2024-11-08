
namespace StreamStore.InMemory
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UseInMemoryDatabase(this IStreamStoreConfigurator configurator)
        {
            configurator.WithDatabase<InMemoryStreamDatabase>();

            return configurator;
        }
    }
}
