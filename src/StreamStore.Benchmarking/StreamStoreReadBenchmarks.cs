using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Dapper.Extensions.Factory;



namespace StreamStore.Benchmarking
{
    public class StreamStoreReadBenchmarks: StreamStoreBenchmarksBase
    {
        [Benchmark]
        public async Task InMemory_ReadRandomStreamWith10Events()
        {
            await ReadRandomStreamWith10Events(inMemoryStore);
        }

        [Benchmark]
        public async Task Sqllite_ReadRandomStreamWith10Events()
        {
            await DapperFactory.Step(async dapper =>
            {
                var store = CreateSqliteStore(dapper);
                await ReadRandomStreamWith10Events(store);
            });
        }

        async Task ReadRandomStreamWith10Events(StreamStore store)
        {
            var streamIdIndex = RandomNumberGenerator.GetInt32(0, streamIds.Length);
            await store.GetAsync(streamIds[streamIdIndex], CancellationToken.None);
        }
    }
}
