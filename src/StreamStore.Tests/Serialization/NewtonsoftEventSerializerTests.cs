﻿using StreamStore.Serialization;
using StreamStore.Testing;


namespace StreamStore.Tests.Serialization
{
    public class NewtonsoftEventSerializerTests : EventSerializerTestsBase
    {
        protected override IEventSerializer CreateEventSerializer(bool compression)
        {
           return new NewtonsoftEventSerializer(registry, compression);
        }
    }
}