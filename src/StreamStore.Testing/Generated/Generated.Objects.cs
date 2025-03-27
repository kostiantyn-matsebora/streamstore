using System.Linq;
using AutoFixture;
using StreamStore.Serialization;

namespace StreamStore.Testing
{
    public static partial class Generated
    {

        public static class Objects
        {
            public static byte[] ByteArray => Converter.ToByteArray(Primitives.String);

            public static T Single<T>() => new Fixture().Create<T>();

            public static T[] Many<T>(int count)
            {
                return new Fixture().CreateMany<T>(count).ToArray();
            }
        }
    }
}
