using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization.Protobuf;
using StreamStore.Testing.Serializer;


namespace StreamStore.Serialization.Tests.Protobuf
{
    public partial class ProtobufTestEnvironment : SerializerTestEnvironmentBase
    {
        protected override IEventSerializer CreateSerializer(IServiceProvider services)
        {
            return new ProtobufEventSerializer(services.GetRequiredService<ITypeRegistry>(), false);
        }
    }
}
