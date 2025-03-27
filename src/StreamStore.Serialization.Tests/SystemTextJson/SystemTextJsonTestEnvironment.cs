using Microsoft.Extensions.DependencyInjection;
using StreamStore.Testing.Serializer;


namespace StreamStore.Serialization.Tests.SystemTextJson
{
    public partial class SystemTextJsonTestEnvironment : SerializerTestEnvironmentBase
    {
        protected override IEventSerializer CreateSerializer(IServiceProvider services)
        {
            return new SystemTextJsonEventSerializer(services.GetRequiredService<ITypeRegistry>(), false);
        }
    }
}
