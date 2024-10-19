﻿using StreamStore.Serialization;
using StreamStore.Testing;


namespace StreamStore.Tests.Serialization
{
    public class SystemTextJsonEventSerializerTests : EventSerializerTestsBase
    {
        protected override IEventSerializer CreateEventSerializer(bool compression)
        {
           return new SystemTextJsonEventSerializer(registry, compression);
        }
    }
}