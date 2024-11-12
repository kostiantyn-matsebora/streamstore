
using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;


namespace StreamStore
{

    public class StreamStoreConfigurator : IStreamStoreConfigurator
    {
        StreamReadingMode mode = StreamReadingMode.Default;
        IStreamDatabaseRegistrator registrator = new SingleTenantRegistration();

        Action<IServiceCollection> eventSerializerRegistrator = services => services.AddSingleton<IEventSerializer, NewtonsoftEventSerializer>();
        Action<IServiceCollection> typeRegistryRegistrator = services => services.AddSingleton<ITypeRegistry, TypeRegistry>();

        bool compression = false;
        int pageSize = 10;

        public StreamStoreConfigurator()
        {
        }

        public IStreamStoreConfigurator WithReadingMode(StreamReadingMode mode)
        {
            this.mode = mode;
            return this;
        }

        public IStreamStoreConfigurator WithReadingPageSize(int pageSize)
        {
            this.pageSize = pageSize;
            return this;
        }

        public IStreamStoreConfigurator WithTypeRegistry(ITypeRegistry registry)
        {
            typeRegistryRegistrator = services => services.AddSingleton<ITypeRegistry>(registry);
            return this;
        }

        public IStreamStoreConfigurator WithTypeRegistry<T>() where T : ITypeRegistry
        {
            typeRegistryRegistrator = services => services.AddSingleton(typeof(ITypeRegistry), typeof(T));
            return this;
        }

        public IStreamStoreConfigurator WithEventSerializer(IEventSerializer eventSerializer)
        {
            eventSerializerRegistrator = services => services.AddSingleton<IEventSerializer>(eventSerializer);
            return this;
        }

        public IStreamStoreConfigurator WithEventSerializer<TSerialzier>() where TSerialzier : IEventSerializer
        {
            eventSerializerRegistrator = services => services.AddSingleton(typeof(IEventSerializer), typeof(TSerialzier));
            return this;
        }

        public IStreamStoreConfigurator WithCompression()
        {
            this.compression = true;
            return this;
        }

        public IStreamStoreConfigurator EnableMultitenancy()
        {
            registrator = new MultitenantRegistration();
            return this;
        }

        public IStreamStoreConfigurator WithDatabase(Action<IStreamDatabaseRegistrator> registrator)
        {
            registrator(this.registrator);
            return this;
        }

        public IServiceCollection Configure(IServiceCollection services)
        {
            var configuration = new StreamStoreConfiguration
            {
                ReadingMode = mode,
                ReadingPageSize = pageSize,
                Compression = compression
            };

            registrator.Register(services);

            eventSerializerRegistrator(services);
            typeRegistryRegistrator(services);

            services
                .AddSingleton(configuration)
                .AddSingleton<StreamEventEnumeratorFactory>()
                .AddSingleton<EventConverter>()
                .AddSingleton<IStreamStore, StreamStore>();

            return services;
        }
    }
}
