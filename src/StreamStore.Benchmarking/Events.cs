using System;
using ProtoBuf;

namespace StreamStore.Benchmarking
{
    [ProtoContract]
    public partial class LeafEvent
    {
        [ProtoMember(1)]
        public required string Name { get; set; }
        [ProtoMember(2)]
        public int Value { get; set; }
        [ProtoMember(3)]
        public string NullValue { get; set; }
    }

    [ProtoContract]
    public partial class BranchEvent
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public required string Name { get; set; }
        [ProtoMember(3)]
        public LeafEvent[] Leaves { get; set; }
    }

    [ProtoContract]
    public partial class RootEvent
    {
        [ProtoMember(1)]
        public required BranchEvent[] Branches { get; set; }
        [ProtoMember(2)]
        public DateTime Timestamp { get; set; }
        [ProtoMember(3)]
        public long Value { get; set; }
    }
}
