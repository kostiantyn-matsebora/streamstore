using StreamStore.Testing.Serializer.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Serialization.Tests.Protobuf
{
    public class Serializing : Serializing<ProtobufTestEnvironment>
    {
        public Serializing(ITestOutputHelper output) : base(output, new ProtobufTestEnvironment())
        {
        }
    }
}
