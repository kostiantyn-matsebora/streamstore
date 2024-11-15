using StreamStore.Testing.Serializer.Scenarios;

namespace StreamStore.Serialization.Tests.SystemTextJson
{
    public class Deserializing : Deserializing<SystemTextJsonTestSuite>
    {
        public Deserializing() : base(new SystemTextJsonTestSuite())
        {
        }
    }
}
