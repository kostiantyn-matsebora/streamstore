using System.Linq;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using StreamStore.Serialization;
using StreamStore.Serialization.SharpSerializer;

namespace StreamStore.Benchmarking
{

    public class EventSerializationBenchmarks
    {
        readonly RootEvent @event;

        readonly SharpEventSerializer sharpSerializer = new SharpEventSerializer();


        [Params(true, false)]
        public bool Compress { get; set; }

        public EventSerializationBenchmarks()
        {
            var fixture = new Fixture();
            @event = fixture.Create<RootEvent>();
        }

        [Benchmark]
        public byte[] SystemTextJsonSerializer()
        { 
          var serializer = new SystemTextJsonEventSerializer(Compress);
          return serializer.Serialize(@event);
        }

        [Benchmark]
        public byte[] NewtonsoftEventSerializer()
        {
            var serializer = new NewtonsoftEventSerializer(Compress);
            return serializer.Serialize(@event);
        }


        [Benchmark]
        public byte[] SharpEventSerializer()
        {
            return sharpSerializer.Serialize(@event);
        }
    }
}
