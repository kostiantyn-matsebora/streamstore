using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;
using StreamStore.Testing.Framework;

namespace StreamStore.Testing.Serializer
{
    public abstract partial class SerializerSuiteBase : TestSuiteBase
    {

        public IEventSerializer Serializer => Services.GetRequiredService<IEventSerializer>();

        public abstract byte[] SerializedEvent { get; }

      
        protected abstract IEventSerializer CreateSerializer(IServiceProvider services);

        protected override void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<ITypeRegistry, TypeRegistry>();
            services.AddSingleton(CreateSerializer);
        }
    }
}
