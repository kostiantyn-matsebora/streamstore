using StreamStore.Testing.StreamStore;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Stream;
using StreamStore.InMemory.Extensions;


namespace StreamStore.Tests.Enumerator
{
    public class EnumerableTestEnvironment : StreamStoreTestEnvironmentBase
    {
        readonly StreamReadingMode mode;

        public EnumerableTestEnvironment(): this(StreamReadingMode.Default)
        {
        }

        public EnumerableTestEnvironment(StreamReadingMode mode)
        {
            this.mode = mode;
        }

        internal IAsyncEnumerable<IStreamEventEnvelope> CreateEnumerable(StreamReadingParameters parameters)
        {
            return new StreamEventEnumerable(parameters, Services.GetRequiredService<StreamEventEnumeratorFactory>());
        }

        protected override void ConfigureStreamStore(IStreamStoreConfigurator configurator)
        {
            configurator.WithReadingMode(mode);
            configurator.WithSingleStorage(x => x.UseInMemoryStorage());
        }
    }
}
