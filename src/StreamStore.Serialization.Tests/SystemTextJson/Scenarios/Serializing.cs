using StreamStore.Testing.Serializer.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Serialization.Tests.SystemTextJson
{
    public class Serializing : Serializing<SystemTextJsonTestEnvironment>
    {
        public Serializing(ITestOutputHelper output) : base(output, new SystemTextJsonTestEnvironment())
        {
        }
    }
}
