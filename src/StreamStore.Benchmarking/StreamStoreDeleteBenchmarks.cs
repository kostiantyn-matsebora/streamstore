using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;



namespace StreamStore.Benchmarking
{
    public class StreamStoreDeleteBenchmarks: StreamStoreBenchmarksBase
    {
        [Benchmark]
        public async Task DeleteRandomStreamWith10Events()
        {
            var streamIdIndex = RandomNumberGenerator.GetInt32(0, streamIds.Length);
            await store.DeleteAsync(streamIds[streamIdIndex], CancellationToken.None);
        }
    }
}
