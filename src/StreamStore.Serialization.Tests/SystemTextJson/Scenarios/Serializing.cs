using StreamStore.Testing.Serializer.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Serialization.Tests.SystemTextJson.Scenarios
{
    public class Serializing : Serializing<SystemTextJsonTestSuite>
    {
        public Serializing(ITestOutputHelper output) : base(output, new SystemTextJsonTestSuite())
        {
        }
    }
}
