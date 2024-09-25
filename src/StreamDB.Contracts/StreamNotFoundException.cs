using System;

namespace StreamDB
{
    [Serializable]
    public class StreamNotFoundException : StreamDBException
    {
        public string StreamId { get; set; }

        public StreamNotFoundException(string id): base(string.Format("Stream {0} is not found."))
        {
            StreamId = id;
        }
    }
}