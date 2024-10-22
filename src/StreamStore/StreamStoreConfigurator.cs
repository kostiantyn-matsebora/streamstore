
using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;


namespace StreamStore
{

    public class StreamStoreConfigurator: IStreamStoreConfigurator {
        StreamReadingMode mode = StreamReadingMode.Default;

        Type eventSerializerType = typeof(NewtonsoftEventSerializer);
        IEventSerializer? eventSerializer;

        Type typeRegistryType = typeof(TypeRegistry);
        ITypeRegistry? typeRegistry;

        bool compression = false;
        int pageSize = 10;
        
        readonly IServiceCollection services;
        public IServiceCollection Services => services;

        public StreamStoreConfigurator(IServiceCollection services)
        {
            this.services = services?? throw new ArgumentNullException(nameof(services));
        }

        public IStreamStoreConfigurator WithReadingMode(StreamReadingMode mode) {
            this.mode = mode;
            return this;
        }

        public IStreamStoreConfigurator WithReadingPageSize(int pageSize) {
            this.pageSize = pageSize;
            return this;
        }

        public IStreamStoreConfigurator WithTypeRegistry(ITypeRegistry registry)  {
            typeRegistry = registry;
            return this;
        }

        public IStreamStoreConfigurator WithTypeRegistry<T>() where T : ITypeRegistry {
            typeRegistryType = typeof(T);
            return this;
        }

        public IStreamStoreConfigurator WithEventSerializer(IEventSerializer eventSerializer) {
            this.eventSerializer = eventSerializer;
            return this;
        }
        
        public IStreamStoreConfigurator WithEventSerializer<TSerialzier>() where TSerialzier : IEventSerializer {
            this.eventSerializerType = typeof(TSerialzier);
            return this;
        }
        
        public IStreamStoreConfigurator WithCompression() 
        {
            this.compression = true;
            return this;
        }

        internal void Configure() {

            var configuration = new StreamStoreConfiguration 
            {
                Mode = mode,
                ReadingPageSize = pageSize,
                Compression = compression
            };

            if (eventSerializer == null) {
                services.AddSingleton(typeof(IEventSerializer), eventSerializerType);
            } else {
                services.AddSingleton(typeof(IEventSerializer), eventSerializer);
            }

            if (typeRegistry == null) {
                services.AddSingleton(typeof(ITypeRegistry), typeRegistryType);
            } else {
                services.AddSingleton(typeof(ITypeRegistry), typeRegistry);
            }

            services
                .AddSingleton(configuration)
                .AddSingleton<IStreamStore, StreamStore>();
        }

    }
}
