using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using StreamStore.Serialization;

namespace StreamStore.Benchmarking
{
    public class TypeRegistryBenchmarks
    {
        readonly Type[] types;
        readonly string[] names;
        readonly TypeRegistry typeRegistry = TypeRegistry.CreateAndInitialize();

        public TypeRegistryBenchmarks()
        {
           types = AppDomain.CurrentDomain.GetAssemblies()
             .Where(a => !a.IsDynamic)
             .SelectMany(a => a.GetTypes())
             .ToArray();
           names = types.Select(t => t.FullName).ToArray();
        }

        [Benchmark]
        public void ResolveNameByType()
        {
            var random = new Random();
            typeRegistry.ResolveNameByType(types[random.Next(0, types.Length - 1)]);
        }

        [Benchmark]
        public void ResolveTypeByName()
        {
            var random = new Random();
            typeRegistry.ResolveTypeByName(names[random.Next(0, types.Length - 1)]);
        }
    }
}
