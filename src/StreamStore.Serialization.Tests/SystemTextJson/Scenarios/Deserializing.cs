using StreamStore.Testing.Serializer.Scenarios;

namespace StreamStore.Serialization.Tests.SystemTextJson
{
    public class Deserializing : Deserializing<SystemTextJsonTestEnvironment>
    {
        public Deserializing() : base(new SystemTextJsonTestEnvironment())
        {
        }
    }
}
