using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using StreamStore.InMemory;


namespace StreamStore.Benchmarking
{
    public class StreamStoreDeleteBenchmarks
    {
        readonly Event[] events;
        readonly string[] streamIds;
        readonly IStreamStore store;

        public StreamStoreDeleteBenchmarks() {

            var fixture = new Fixture();
            events = fixture.CreateMany<Event>(10).ToArray();
            store = new StreamStore(new InMemoryStreamDatabase());

            streamIds = Enumerable
                .Range(0, 1000)
                .Select(i => Guid.NewGuid().ToString())
                .ToArray();

            Parallel.ForEach(streamIds, streamId =>
            {
                store
                    .OpenStreamAsync(streamId, CancellationToken.None)
                    .AddRangeAsync(events, CancellationToken.None)
                    .SaveChangesAsync(CancellationToken.None).Wait();
            });
        }

        [Benchmark]
        public async Task DeleteRandomStreamWith10Events()
        {
            var streamIdIndex = new Random().Next(0, streamIds.Length);
            await store.DeleteAsync(streamIds[streamIdIndex], CancellationToken.None);
        }
    }
}
