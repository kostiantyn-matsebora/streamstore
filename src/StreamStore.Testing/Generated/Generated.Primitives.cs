using System;
using AutoFixture;


namespace StreamStore.Testing
{
    public static partial class Generated
    {
        public static class Primitives
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
                    return new Revision(new Random().Next(0, Int32.MaxValue));
                }
            }

            public static Guid Guid
            {
                get
                {
                    return Guid.NewGuid();
                }
            }
        }
    }
}
