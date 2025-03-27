using StreamStore.Testing.Serializer.Scenarios;

namespace StreamStore.Serialization.Tests.Protobuf
{
    public class Deserializing : Deserializing<ProtobufTestEnvironment>
    {
        public Deserializing() : base(new ProtobufTestEnvironment())
        {
        }
    }
}
