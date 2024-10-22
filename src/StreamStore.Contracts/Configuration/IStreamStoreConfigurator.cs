﻿using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;


namespace StreamStore
{
    public interface IStreamStoreConfigurator
    {
        public IServiceCollection Services { get; }

        public IStreamStoreConfigurator WithReadingMode(StreamReadingMode mode);


        public IStreamStoreConfigurator WithReadingPageSize(int pageSize);

        public IStreamStoreConfigurator WithTypeRegistry(ITypeRegistry registry);


        public IStreamStoreConfigurator WithTypeRegistry<T>() where T : ITypeRegistry;


        public IStreamStoreConfigurator WithEventSerializer(IEventSerializer eventSerializer);

        public IStreamStoreConfigurator WithEventSerializer<TSerialzier>() where TSerialzier : IEventSerializer;
    }
}
