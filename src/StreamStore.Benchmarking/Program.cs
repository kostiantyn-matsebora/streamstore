using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace StreamStore.Benchmarking
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var config = DefaultConfig.Instance;

            // Use this to select benchmarks from the console:
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, config);
        }
    }
}