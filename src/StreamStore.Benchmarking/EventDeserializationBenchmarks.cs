using System.Collections.Generic;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using StreamStore.Serialization;
using StreamStore.Serialization.Protobuf;


namespace StreamStore.Benchmarking
{

    public class EventDeserializationBenchmarks
    {
        readonly Dictionary<bool,byte[]> protobufEvents = new Dictionary<bool, byte[]>();
        readonly Dictionary<bool, byte[]> systemTextJsonEvents = new Dictionary<bool, byte[]>();
        readonly Dictionary<bool, byte[]> newtonsoftEvents = new Dictionary<bool, byte[]>();
        readonly TypeRegistry registry;

        public EventDeserializationBenchmarks()
        {
            registry = TypeRegistry.CreateAndInitialize();

            var fixture = new Fixture();
            var @event = fixture.Create<RootEvent>();
            protobufEvents.Add(true, new ProtobufEventSerializer(registry, true).Serialize(@event));
            protobufEvents.Add(false, new ProtobufEventSerializer(registry, false).Serialize(@event));
            systemTextJsonEvents.Add(true, new SystemTextJsonEventSerializer(registry, true).Serialize(@event));
            systemTextJsonEvents.Add(false, new SystemTextJsonEventSerializer(registry, false).Serialize(@event));
            newtonsoftEvents.Add(true, new NewtonsoftEventSerializer(registry, true).Serialize(@event));
            newtonsoftEvents.Add(false, new NewtonsoftEventSerializer(registry, false).Serialize(@event));
        }

        [Params(true, false)]
        public bool Compress { get; set; }

        [Benchmark]
        public void SystemTextJsonSerializer() 
        {
            var serializer = new SystemTextJsonEventSerializer(registry, Compress);
            serializer.Deserialize(systemTextJsonEvents[Compress]);
        }

        [Benchmark]
        public void NewtonsoftEventSerializer()
        {
            var serializer = new NewtonsoftEventSerializer(registry, Compress);
            serializer.Deserialize(newtonsoftEvents[Compress]);
        }


        [Benchmark]
        public void ProtobufEventSerializer()
        {
            var serializer = new ProtobufEventSerializer(registry, Compress);
            serializer.Deserialize(protobufEvents[Compress]);
        }
    }
}
