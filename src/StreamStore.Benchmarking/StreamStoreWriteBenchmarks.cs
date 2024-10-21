using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;



namespace StreamStore.Benchmarking
{
    public class StreamStoreWriteBenchmarks: StreamStoreBenchmarksBase
    {
        [Benchmark]
        public async Task InMemoryStore_Save10Events()
        {
            await Save10Events(inMemoryStore);
        }

        [Benchmark]
        public async Task SqliteStore_Save10Events()
        {
            var store = GetSqliteStore();
            await Save10Events(store);
        }

        async Task Save10Events(IStreamStore store)
        {
            var streamId = Guid.NewGuid().ToString();

            await store
                .OpenAsync(streamId, CancellationToken.None)
                .WriteAsync(events.Take(10), CancellationToken.None);
        }
    }
}
