
using ProtoBuf;

namespace StreamStore.Serialization.Protobuf
{
    [ProtoContract]
    public partial class ProtobufEventEnvelope
    {
        [ProtoMember(1)]
        public string Type { get; set; }

        [ProtoMember(2)]
        public byte[] Data { get; set; }
    }
}
