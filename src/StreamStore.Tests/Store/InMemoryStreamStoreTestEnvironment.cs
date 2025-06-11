using StreamStore.InMemory.Extensions;
using StreamStore.Testing.StreamStore;


namespace StreamStore.Tests
{
    public class InMemoryStreamStoreTestEnvironment : StreamStoreTestEnvironmentBase
    {
        public InMemoryStreamStoreTestEnvironment()
        {
        }

        protected override void ConfigureStreamStore(IStreamStoreConfigurator configurator)
        {
            configurator.ConfigurePersistence(x => x.UseInMemoryStorage());
        }
    }
}
