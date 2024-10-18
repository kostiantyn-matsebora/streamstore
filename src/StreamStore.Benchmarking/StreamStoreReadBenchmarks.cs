using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;



namespace StreamStore.Benchmarking
{
    public class StreamStoreReadBenchmarks: StreamStoreBenchmarksBase
    {
        [Benchmark]
        public async Task ReadRandomStreamWith10Events()
        {
            var streamIdIndex = RandomNumberGenerator.GetInt32(0, streamIds.Length);
            await store.GetAsync(streamIds[streamIdIndex], CancellationToken.None);
        }
    }
}
