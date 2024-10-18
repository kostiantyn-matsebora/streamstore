using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using StreamStore.InMemory;
using StreamStore.Serialization;


namespace StreamStore.Benchmarking
{
    public class StreamStoreWriteBenchmarks
    {
        readonly Event[] events;
        readonly IStreamStore store;
        readonly TypeRegistry registry;
        public StreamStoreWriteBenchmarks() {
            var fixture = new Fixture();

            registry = TypeRegistry.CreateAndInitialize();
            events = fixture.CreateMany<Event>(100).ToArray();
            store = new StreamStore(new InMemoryStreamDatabase(), new NewtonsoftEventSerializer(registry));
        }

        [Benchmark]
        public async Task Save10Events10Times()
        {
            var streamId = Guid.NewGuid().ToString();
            for (var i = 0; i < 10; i++)
            {
                await store
                    .OpenStreamAsync(streamId, 10 * i, CancellationToken.None)
                     .AddRangeAsync(events.Skip(10 * i).Take(10), CancellationToken.None)
                    .SaveChangesAsync(CancellationToken.None);
            }
        }
    }
}
