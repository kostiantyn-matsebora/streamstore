
using StreamStore.Testing.Serializer.Scenarios;

namespace StreamStore.Serialization.Tests.NewtonsoftJson.Scenarios
{
    public class Deserializing : Deserializing<NewtonsoftTestSuite>
    {
        public Deserializing() : base(new NewtonsoftTestSuite())
        {
        }
    }
}
