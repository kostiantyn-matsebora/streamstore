using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Serializer;


namespace StreamStore.Serialization.Tests.SystemTextJson
{
    public partial class SystemTextJsonTestSuite : SerializerSuiteBase
    {
        protected override IEventSerializer CreateSerializer(IServiceProvider services)
        {
            return new SystemTextJsonEventSerializer(services.GetRequiredService<ITypeRegistry>(), true);
        }
    }
}
