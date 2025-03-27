
using StreamStore.Testing.Serializer.Scenarios;

namespace StreamStore.Serialization.Tests.NewtonsoftJson
{
    public class Deserializing : Deserializing<NewtonsoftTestEnvironment>
    {
        public Deserializing() : base(new NewtonsoftTestEnvironment())
        {
        }
    }
}
