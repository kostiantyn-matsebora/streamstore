﻿using System;

namespace StreamStore
{
    [Serializable]
    public class StreamNotFoundException : StreamStoreException
    {
        public string StreamId { get; set; }

        public StreamNotFoundException(string id) : base(string.Format("Stream {0} is not found."))
        {
            StreamId = id;
        }
    }
}