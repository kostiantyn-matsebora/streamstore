

using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;
using StreamStore.Testing.Framework;

namespace StreamStore.Testing.Scenarios.Serializer
{
    public abstract class SerializerSuiteBase : TestSuiteBase
    {

        public IEventSerializer Serializer => Services.GetRequiredService<IEventSerializer>();

        public abstract byte[] SerializedEvent { get; }

        public abstract object DeserializedEvent { get; }

        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ITypeRegistry, TypeRegistry>();
        }
    }
}
