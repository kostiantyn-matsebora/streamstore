using StreamStore.InMemory;
using StreamStore.Testing.StreamStore;

namespace StreamStore.Tests.Scenarios
{
    public class StreamStoreSuite : StreamStoreSuiteBase
    {
        protected override void ConfigureStreamStore(IStreamStoreConfigurator configurator)
        {
            configurator.UseMemoryStreamDatabase();
        }
    }
}
