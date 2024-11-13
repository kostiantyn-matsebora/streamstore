using StreamStore.InMemory.Extensions;
using StreamStore.Testing.StreamStore;


namespace StreamStore.Tests
{
    public class InMemoryStreamStoreSuite : StreamStoreSuiteBase
    {
        public InMemoryStreamStoreSuite()
        {
        }

        protected override void ConfigureStreamStore(IStreamStoreConfigurator configurator)
        {
            configurator.WithSingleDatabse(x => x.UseInMemoryDatabase());
        }
    }
}
