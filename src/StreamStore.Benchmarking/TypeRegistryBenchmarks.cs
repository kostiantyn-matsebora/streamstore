using System;
using System.Linq;
using System.Security.Cryptography;
using BenchmarkDotNet.Attributes;
using StreamStore.Serialization;

namespace StreamStore.Benchmarking
{
    public class TypeRegistryBenchmarks
    {
        readonly Type[] types;
        readonly string[] names;
        readonly TypeRegistry typeRegistry;

        public TypeRegistryBenchmarks()
        {
           typeRegistry = new TypeRegistry();
           types = AppDomain.CurrentDomain.GetAssemblies()
             .Where(a => !a.IsDynamic)
             .SelectMany(a => a.GetTypes())
             .ToArray();
           names = types.Select(t => t.FullName).ToArray();
        }

        [Benchmark]
        public void ResolveNameByType()
        {
            typeRegistry.ResolveNameByType(types[RandomNumberGenerator.GetInt32(0, types.Length - 1)]);
        }

        [Benchmark]
        public void ResolveTypeByName()
        {
            typeRegistry.ResolveTypeByName(names[RandomNumberGenerator.GetInt32(0, types.Length - 1)]);
        }
    }
}
