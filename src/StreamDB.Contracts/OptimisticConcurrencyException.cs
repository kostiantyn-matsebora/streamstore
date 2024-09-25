﻿using System;


namespace StreamDB
{
    [Serializable]
    public sealed class OptimisticConcurrencyException : StreamDBException
    {
        public int ExpectedRevision { get; set; }
        public int ActualRevision { get; set;  }

        public Id StreamId { get; }
        public OptimisticConcurrencyException(int expectedRevision, int actualRevision, Id streamId): base("Stream has been already changed, your version is stale.")
        {
            ExpectedRevision = expectedRevision;
            ActualRevision = actualRevision;
            StreamId = streamId;
        }
    }
}