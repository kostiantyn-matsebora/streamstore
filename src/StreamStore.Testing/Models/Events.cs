using System;

namespace StreamStore.Testing
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public class LeafEvent
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public string? NullValue { get; set; }
    }

    public class BranchEvent
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public LeafEvent[] Leaves { get; set; }
    }

    public class RootEvent
    {

        public BranchEvent[] Branches { get; set; }


        public DateTime Timestamp { get; set; }

        public long Value { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
