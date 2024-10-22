
using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;

namespace StreamStore
{

    public class StreamStoreConfigurator {
        StreamReadingMode mode = StreamReadingMode.Default;

        Type eventSerializerType = typeof(NewtonsoftEventSerializer);
        IEventSerializer? eventSerializer;

        Type typeRegistryType = typeof(TypeRegistry);
        ITypeRegistry? typeRegistry;

        bool compression = false;        
        int pageSize = 10;

        public StreamStoreConfigurator WithReadingMode(StreamReadingMode mode) {
            this.mode = mode;
            return this;
        }

        public StreamStoreConfigurator WithReadingPageSize(int pageSize) {
            this.pageSize = pageSize;
            return this;
        }

        public StreamStoreConfigurator WithTypeRegistry(ITypeRegistry registry)  {
            typeRegistry = registry;
            return this;
        }

        public StreamStoreConfigurator WithTypeRegistry<T>() where T : ITypeRegistry {
            typeRegistryType = typeof(T);
            return this;
        }

        public StreamStoreConfigurator WithEventSerializer(IEventSerializer eventSerializer) {
            this.eventSerializer = eventSerializer;
            return this;
        }
        
        public StreamStoreConfigurator WithEventSerializer<TSerialzier>() where TSerialzier : IEventSerializer {
            this.eventSerializerType = typeof(TSerialzier);
            return this;
        }
        
        public StreamStoreConfigurator WithCompression() 
        {
            this.compression = true;
            return this;
        }

        internal void Configure(IServiceCollection services) {
            var configuration = new StreamStoreConfiguration 
            {
                Mode = mode,
                ReadingPageSize = pageSize
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

            services.AddSingleton(configuration)
                     .AddSingleton<IStreamStore, StreamStore>();
        }

    }
}
