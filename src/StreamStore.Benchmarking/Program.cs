using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace StreamStore.Benchmarking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = DefaultConfig.Instance;
            //var summary = BenchmarkRunner.Run<SerializerBenchmarks>(config, args);

            // Use this to select benchmarks from the console:
             var summaries = BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);

            //var benchmark = new InMemoryStreamStoreBenchmark();
            //benchmark.AppendTenEventsTenTimes().Wait();
        }
    }
}