
using System;
using System.Linq;
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
        Action<IServiceCollection>? databaseRegistrator;

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

        public IServiceCollection Configure(IServiceCollection services)
        {

            if (databaseType == null && databaseRegistrator == null)
            {
                throw new InvalidOperationException("Database backend is not set");
            }

            var configuration = new StreamStoreConfiguration
            {
                ReadingMode = mode,
                ReadingPageSize = pageSize,
                Compression = compression
            };

            RegisterEventSerializer(services);
            RegisterTypeRegistry(services);
            RegisterDatabase(services);

            services
                .AddSingleton(configuration)
                .AddSingleton<StreamEventEnumeratorFactory>()
                .AddSingleton<EventConverter>()
                .AddSingleton<IStreamStore, StreamStore>();

            return services;
        }

        private void RegisterEventSerializer(IServiceCollection services)
        {
            if (eventSerializer == null)
            {
                services.AddSingleton(typeof(IEventSerializer), eventSerializerType);
            }
            else
            {
                services.AddSingleton(typeof(IEventSerializer), eventSerializer);
            }
        }

        public IStreamStoreConfigurator WithDatabase<T>() where T : IStreamDatabase
        {
            databaseType = typeof(T);
            return this;
        }


        public IStreamStoreConfigurator WithDatabase(Action<IServiceCollection> registrator)
        {
            databaseRegistrator = registrator;
            return this;
        }

        void RegisterTypeRegistry(IServiceCollection services)
        {
            if (typeRegistry == null)
            {
                services.AddSingleton(typeof(ITypeRegistry), typeRegistryType);
            }
            else
            {
                services.AddSingleton(typeof(ITypeRegistry), typeRegistry);
            }
        }

        void RegisterDatabase(IServiceCollection services)
        {
            if (databaseRegistrator != null)
            {
                databaseRegistrator(services);

                if (!services.Any(s => s.ServiceType == typeof(IStreamDatabase)))
                {
                    throw new InvalidOperationException("Database backend (IStreamDatabase) is not registered");
                }

                if (!services.Any(s => s.ServiceType == typeof(IStreamReader)))
                {
                    throw new InvalidOperationException("Database backend (IStreamReader) is not registered");
                }

            }
            else
            {
                services.AddSingleton(typeof(IStreamDatabase), databaseType!);
                services.AddSingleton(typeof(IStreamReader), services => services.GetRequiredService<IStreamDatabase>());
            }
        }

    }
}
