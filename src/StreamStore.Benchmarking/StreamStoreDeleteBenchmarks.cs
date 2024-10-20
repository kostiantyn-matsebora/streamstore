using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Dapper.Extensions.Factory;

namespace StreamStore.Benchmarking
{
    public class StreamStoreDeleteBenchmarks: StreamStoreBenchmarksBase
    {
        [Benchmark]
        public async Task InMemory_DeleteRandomStreamWith10Events()
        {
            await DeleteRandomStreamWith10Events(inMemoryStore);
        }
        [Benchmark]
        public  async Task Sqllite_DeleteRandomStreamWith10Events()
        {
            await DapperFactory.Step(async dapper => 
            {
                var store = CreateSqliteStore(dapper);
                await DeleteRandomStreamWith10Events(store);
            });
        }

        async Task DeleteRandomStreamWith10Events(StreamStore store)
        {
            var streamIdIndex = RandomNumberGenerator.GetInt32(0, streamIds.Length);
            await store.DeleteAsync(streamIds[streamIdIndex], CancellationToken.None);
        }
    }
}
