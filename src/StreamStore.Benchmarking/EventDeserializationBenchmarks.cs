using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using StreamStore.Serialization;
using StreamStore.Serialization.SharpSerializer;

namespace StreamStore.Benchmarking
{

    public class EventDeserializationBenchmarks
    {
        readonly SharpEventSerializer sharpSerializer = new SharpEventSerializer();
        readonly SystemTextJsonEventSerializer systemTextJsonEventSerializer = new SystemTextJsonEventSerializer();
        readonly NewtonsoftEventSerializer newtonsoftEventSerializer = new NewtonsoftEventSerializer();
        readonly byte[] sharpSerializedEvent;
        readonly byte[] systemTextJsonSerializedEvent;
        readonly byte[] newtonsoftSerializedEvent;

        public EventDeserializationBenchmarks()
        {
            var fixture = new Fixture();
            var @event = fixture.Create<RootEvent>();
            sharpSerializedEvent = sharpSerializer.Serialize(@event);
            systemTextJsonSerializedEvent = systemTextJsonEventSerializer.Serialize(@event);
            newtonsoftSerializedEvent = newtonsoftEventSerializer.Serialize(@event);
        }

        [Benchmark]
        public void SystemTextJsonSerializer_Deserialize100Events() 
        {
           systemTextJsonEventSerializer.Deserialize(systemTextJsonSerializedEvent);
        }

        [Benchmark]
        public void NewtonsoftEventSerializer_Deserialize100Events()
        {
           newtonsoftEventSerializer.Deserialize(newtonsoftSerializedEvent);
        }


        [Benchmark]
        public void SharpEventSerializer_Deserialize100Events()
        {
           sharpSerializer.Deserialize(sharpSerializedEvent);
        }
    }
}
