using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;


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
            var store = GetSqliteStore();
            await ReadRandomStreamWith10Events(store);
        }

        async Task ReadRandomStreamWith10Events(IStreamStore store)
        {
            var streamIdIndex = RandomNumberGenerator.GetInt32(0, streamIds.Length);
            await store.OpenAsync(streamIds[streamIdIndex]).ReadToEnd(CancellationToken.None);
        }
    }
}
