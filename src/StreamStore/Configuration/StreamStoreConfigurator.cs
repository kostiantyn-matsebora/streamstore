
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StreamStore.Provisioning;
using StreamStore.Serialization;


namespace StreamStore
{

    public class StreamStoreConfigurator : IStreamStoreConfigurator
    {
        StreamReadingMode mode = StreamReadingMode.Default;
        IStreamDatabaseRegistrator registrator = new SingleTenantDatabaseRegistrator();

        Action<IServiceCollection> eventSerializerRegistrator = services => services.AddSingleton<IEventSerializer, NewtonsoftEventSerializer>();
        Action<IServiceCollection> typeRegistryRegistrator = services => services.AddSingleton<ITypeRegistry, TypeRegistry>();

        bool compression = false;
        bool schemaProvisioningEnabled = true;

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

        public IStreamStoreConfigurator EnableCompression()
        {
            this.compression = true;
            return this;
        }

        public IStreamStoreConfigurator EnableSchemaProvisioning()
        {
            schemaProvisioningEnabled = true;
            return this;
        }

        public IStreamStoreConfigurator WithSingleDatabase(Action<ISingleTenantDatabaseRegistrator> registrator)
        {
            var singleTenantRegistrator = new SingleTenantDatabaseRegistrator();
            registrator(singleTenantRegistrator);
            this.registrator = singleTenantRegistrator;
            return this;
        }

        public IStreamStoreConfigurator WithMultitenancy(Action<IMultitenantDatabaseRegistrator> registrator)
        {
            var multiTenantregistrator = new MultitenantDatabaseRegistrator();
            registrator(multiTenantregistrator);
            this.registrator = multiTenantregistrator;
            return this;
        }

        public IServiceCollection Configure(IServiceCollection services)
        {
            var configuration = new StreamStoreConfiguration
            {
                ReadingMode = mode,
                ReadingPageSize = pageSize,
                CompressionEnabled = compression,
                SchemaProvisioningEnabled = schemaProvisioningEnabled
            };

            registrator.Register(services, configuration);

            eventSerializerRegistrator(services);
            typeRegistryRegistrator(services);

            RegisterStoreDependencies(services, configuration);

            if (configuration.SchemaProvisioningEnabled)
                services.AddHostedService<SchemaProvisioningService>();

            return services;
        }

        private static void RegisterStoreDependencies(IServiceCollection services, StreamStoreConfiguration configuration)
        {
            services
                .AddSingleton(configuration)
                .AddSingleton<StreamEventEnumeratorFactory>()
                .AddSingleton<EventConverter>()
                .AddSingleton<IStreamStore, StreamStore>();
        }
    }
}
