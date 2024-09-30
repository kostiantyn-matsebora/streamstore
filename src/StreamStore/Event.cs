using System;

namespace StreamStore
{
    public sealed class Event
    {
        public Id Id { get; set; }
        public DateTime Timestamp { get; set; }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public object EventObject { get; set;  }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
