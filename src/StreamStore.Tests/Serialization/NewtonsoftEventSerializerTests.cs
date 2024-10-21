using StreamStore.Serialization;
using StreamStore.Testing.Framework;

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