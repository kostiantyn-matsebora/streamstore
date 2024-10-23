
using StreamStore.Serialization.Tests.NewtonsoftJson;
using StreamStore.Testing.Serializer.Scenarios;

namespace StreamStore.Serialization.Tests.Protobuf.Scenarios
{
    public class Deserializing : Deserializing<ProtobufTestSuite>
    {
        public Deserializing() : base(new ProtobufTestSuite())
        {
        }
    }
}
