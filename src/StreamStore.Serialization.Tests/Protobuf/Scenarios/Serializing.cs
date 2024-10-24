﻿using StreamStore.Testing.Serializer.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Serialization.Tests.Protobuf.Scenarios
{
    public class Serializing : Serializing<ProtobufTestSuite>
    {
        public Serializing(ITestOutputHelper output) : base(output, new ProtobufTestSuite())
        {
        }
    }
}