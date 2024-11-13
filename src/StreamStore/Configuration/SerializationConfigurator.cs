using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;

namespace StreamStore.Configuration
{
    internal class SerializationConfigurator : ISerializationConfigurator
    {
        readonly ServiceCollection services = new ServiceCollection();

        public SerializationConfigurator()
        {
            services.AddSingleton(typeof(IEventSerializer), typeof(NewtonsoftEventSerializer));
            services.AddSingleton(typeof(ITypeRegistry), typeof(TypeRegistry));
        }

        public ISerializationConfigurator EnableCompression()
        {
            return this;
        }


        public ISerializationConfigurator UseSerializer<TSerializer>(Action<IServiceCollection>? dependencies = null) where TSerializer : IEventSerializer
        {
            services.AddSingleton(typeof(IEventSerializer), typeof(TSerializer));
            if (dependencies != null) dependencies.Invoke(services);
            return this;
        }

        public ISerializationConfigurator UseTypeRegistry<TRegistry>() where TRegistry : ITypeRegistry
        {
            services.AddSingleton(typeof(ITypeRegistry), typeof(TRegistry));
            return this;
        }

        public IServiceCollection Configure()
        {
            return services;
        }

        public ISerializationConfigurator UseTypeRegistry<TRegistry>(Action<IServiceCollection>? dependencies = null) where TRegistry : ITypeRegistry
        {
            services.AddSingleton(typeof(ITypeRegistry), typeof(TRegistry));
            if (dependencies != null) dependencies.Invoke(services);
            return this;
        }
    }
}
