﻿using AutoFixture;
using StreamStore.Serialization;


namespace StreamStore.Testing
{
    public static class GeneratedValues
    {
        public static string String => new Fixture().Create<string>();

        public static Id Id => new Fixture().Create<Id>();
        public static DateTime DateTime => DateTime.UtcNow;
        public static int Int
        {
            get
            {
                return new Random().Next(Int32.MinValue, Int32.MaxValue);
            }
        }

        public static Revision Revision
        {
            get
            {
                return Revision.New(new Random().Next(0, Int32.MaxValue));
            }
        }

        public static byte[] ByteArray => Converter.ToByteArray(String);

        public static EventItem[] CreateEventItems(int count)
        {
            return new Fixture().CreateEventItems(count);
        }
    }
}
