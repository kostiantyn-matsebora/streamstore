using StreamStore.Testing.StreamStore;
using StreamStore.InMemory;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Stream;


namespace StreamStore.Tests.Enumerator
{
    public class StreamEventEnumeratorTestSuite : StreamStoreSuiteBase
    {
        readonly StreamReadingMode mode;

        public StreamEventEnumeratorTestSuite(StreamReadingMode mode)
        {
            this.mode = mode;
        }

        internal IAsyncEnumerable<StreamEvent> CreateEnumerable(StreamReadingParameters parameters)
        {
            return new StreamEventEnumerable(parameters, Services.GetRequiredService<StreamEventEnumeratorFactory>());
        }

        protected override void ConfigureStreamStore(IStreamStoreConfigurator configurator)
        {
            configurator.WithReadingMode(mode);
            configurator.UseMemoryStreamDatabase();
        }
    }
}
