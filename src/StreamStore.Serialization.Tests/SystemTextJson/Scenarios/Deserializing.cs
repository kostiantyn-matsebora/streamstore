using StreamStore.Testing.Serializer.Scenarios;

namespace StreamStore.Serialization.Tests.SystemTextJson.Scenarios
{
    public class Deserializing : Deserializing<SystemTextJsonTestSuite>
    {
        public Deserializing() : base(new SystemTextJsonTestSuite())
        {
        }
    }
}
