using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Dapper.Extensions.Factory;
using StreamStore.Stream;


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
            await DapperFactory.Step(async dapper =>
            {
                var store = CreateSqliteStore(dapper);
                await Save10Events(store);
            });
        }


        async Task Save10Events(StreamStore store)
        {
            var streamId = Guid.NewGuid().ToString();

            await store
                .OpenStreamAsync(streamId,  CancellationToken.None)
                .AddRangeAsync(events.Take(10), CancellationToken.None)
                .SaveChangesAsync(CancellationToken.None);
        }
    }
}
