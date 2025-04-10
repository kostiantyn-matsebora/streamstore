using System;

namespace StreamStore.Testing
{
    public class TestEventRecord: IEventRecord
    {
        public TestEventRecord(Id id, DateTime timestamp, byte[] data)
        {
            Id = id;
            Timestamp = timestamp;
            Data = data;
        }

        public byte[] Data { get; }

        public Id Id { get;  }

        public DateTime Timestamp { get; }
    }
}
