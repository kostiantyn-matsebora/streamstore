using System;
using MessagePack;

namespace StreamStore.Benchmarking
{
    [MessagePackObject]
    public class LeafEvent
    {
        public required string Name { get; set; }
        public int Value { get; set; }

        public string NullValue { get; set; }
    }

    [MessagePackObject]
    public class BranchEvent
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public LeafEvent[] Leaves { get; set; }
    }

    [MessagePackObject]
    public class RootEvent
    {
        public required BranchEvent[] Branches { get; set; }

        public DateTime Timestamp { get; set; }

        public long Value { get; set; }
    }
}
