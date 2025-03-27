using StreamStore.Testing.Serializer.Scenarios;
using Xunit.Abstractions;

namespace StreamStore.Serialization.Tests.NewtonsoftJson
{
    public class Serializing : Serializing<NewtonsoftTestEnvironment>
    {

        public Serializing(ITestOutputHelper output) : base(output, new NewtonsoftTestEnvironment())
        {
        }
    }
}
