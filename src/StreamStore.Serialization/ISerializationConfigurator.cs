using System;

using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;

namespace StreamStore
{
    public interface ISerializationConfigurator
    {
        ISerializationConfigurator EnableCompression();
        ISerializationConfigurator UseSerializer<TSerializer>(Action<IServiceCollection>? dependencies = null) where TSerializer : IEventSerializer;

        ISerializationConfigurator UseTypeRegistry<TRegistry>(Action<IServiceCollection>? dependencies = null) where TRegistry : ITypeRegistry;

        IServiceCollection Configure();
    }

}
