﻿using System;

namespace StreamStore
{
    [Serializable]
    public class StreamNotFoundException : StreamStoreException
    {
        public string StreamId { get; set; }

        public StreamNotFoundException(string id) : base($"Stream {id} is not found.")
        {
            StreamId = id;
        }
    }
}