using StreamStore.Testing.Serializer.Scenarios;

namespace StreamStore.Serialization.Tests.Protobuf
{
    public class Deserializing : Deserializing<ProtobufTestSuite>
    {
        public Deserializing() : base(new ProtobufTestSuite())
        {
        }
    }
}
