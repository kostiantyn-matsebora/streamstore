﻿using StreamStore.Testing.Serializer.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Serialization.Tests.NewtonsoftJson.Scenarios
{
    public class Serializing : Serializing<NewtonsoftTestSuite>
    {

        public Serializing(ITestOutputHelper output) : base(output, new NewtonsoftTestSuite())
        {
        }
    }
}