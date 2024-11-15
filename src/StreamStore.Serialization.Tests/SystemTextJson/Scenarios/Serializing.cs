using StreamStore.Testing.Serializer.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Serialization.Tests.SystemTextJson
{
    public class Serializing : Serializing<SystemTextJsonTestSuite>
    {
        public Serializing(ITestOutputHelper output) : base(output, new SystemTextJsonTestSuite())
        {
        }
    }
}
