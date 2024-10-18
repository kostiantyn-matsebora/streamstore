using AutoFixture;
using BenchmarkDotNet.Attributes;
using StreamStore.Serialization;
using StreamStore.Serialization.Protobuf;

namespace StreamStore.Benchmarking
{

    public class EventSerializationBenchmarks
    {
        readonly RootEvent @event;
        readonly TypeRegistry registry;


        [Params(true, false)]
        public bool Compress { get; set; }

        public EventSerializationBenchmarks()
        {
            registry = TypeRegistry.CreateAndInitialize();
            var fixture = new Fixture();
            @event = fixture.Create<RootEvent>();
        }

        [Benchmark]
        public byte[] SystemTextJsonSerializer()
        { 
          var serializer = new SystemTextJsonEventSerializer(registry, Compress);
          return serializer.Serialize(@event);
        }

        [Benchmark]
        public byte[] NewtonsoftEventSerializer()
        {
            var serializer = new NewtonsoftEventSerializer(registry, Compress);
            return serializer.Serialize(@event);
        }


        [Benchmark]
        public byte[] ProtobufEventSerializer()
        {
            var serializer = new ProtobufEventSerializer(registry, Compress);
            return serializer.Serialize(@event);
        }
    }
}
