
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

        Type? databaseType;

        bool compression = false;
        int pageSize = 10;
        
        public StreamStoreConfigurator()
        {
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

        public IServiceCollection Configure(IServiceCollection services) {

            if (databaseType == null)
            {
                throw new InvalidOperationException("Database backend is not set");
            }

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
                .AddSingleton(typeof(IStreamDatabase), databaseType)
                .AddSingleton<IStreamStore, StreamStore>();

            return services;
        }

        public IStreamStoreConfigurator WithDatabase<T>() where T : IStreamDatabase
        {
            databaseType = typeof(T);
            return this;
        }
    }
}
