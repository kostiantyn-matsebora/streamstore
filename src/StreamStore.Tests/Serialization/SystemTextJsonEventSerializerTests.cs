using StreamStore.Serialization.SharpSerializer;
using StreamStore.Testing;


namespace StreamStore.Tests.Serialization
{
    public class SharpEventSerializerTests : EventSerializerTestsBase
    {
        protected override IEventSerializer CreateEventSerializer()
        {
           return new SharpEventSerializer();
        }
    }
}