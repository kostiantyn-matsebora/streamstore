﻿using ProtoBuf;


namespace StreamStore.Tests.Serialization
{


    [ProtoContract]
    public partial class ProtobufLeafEvent
    {
        [ProtoMember(1)]
        public required string Name { get; set; }
        [ProtoMember(2)]
        public int Value { get; set; }
        [ProtoMember(3)]
        public string? NullValue { get; set; }
    }

    [ProtoContract]
    public partial class ProtobufBranchEvent
    {
        [ProtoMember(1)]
        public int Id { get; set; }
        [ProtoMember(2)]
        public required string Name { get; set; }
        [ProtoMember(3)]
        public ProtobufLeafEvent[]? Leaves { get; set; }
    }

    [ProtoContract]
    public partial class ProtobufRootEvent
    {
        [ProtoMember(1)]
        public required ProtobufBranchEvent[] Branches { get; set; }
        [ProtoMember(2)]
        public DateTime Timestamp { get; set; }
        [ProtoMember(3)]
        public long Value { get; set; }
    }
}