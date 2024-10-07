﻿using AutoFixture;
using StreamStore.S3;
using StreamStore.Serialization;

namespace StreamStore.Testing
{
    public static  class RandomValues
    {
        public static string RandomString => new Fixture().Create<string>();

        public static byte[] RandomByteArray => Converter.ToByteArray(RandomString);

        public static EventRecord[] CreateEventRecords(int initialRevision, int count)
        {
            return new Fixture().CreateEventRecords(initialRevision, count);
        }

        public static EventRecord[] CreateEventRecords(int count)
        {
            return new Fixture().CreateEventRecords(count);
        }
    }
}
