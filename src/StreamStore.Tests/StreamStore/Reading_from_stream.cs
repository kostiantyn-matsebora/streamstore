﻿using StreamStore.Testing.StreamStore.Scenarios;

namespace StreamStore.Tests.Scenarios
{
    public class Reading_from_stream: Reading_from_stream<StreamStoreSuite>
    {
        public Reading_from_stream(StreamStoreSuite suite) : base(suite)
        {
        }
    }
}