
namespace StreamStore.InMemory
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator UseMemoryStreamDatabase(this IStreamStoreConfigurator configurator)
        {
            configurator.WithDatabase<InMemoryStreamDatabase>();

            return configurator;
        }
    }
}
