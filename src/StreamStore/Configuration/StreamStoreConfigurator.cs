
using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Provisioning;
using StreamStore.Serialization;


namespace StreamStore
{

    public class StreamStoreConfigurator : IStreamStoreConfigurator
    {
        StreamReadingMode mode = StreamReadingMode.Default;
        IStreamDatabaseRegistrator? dbRegistrator;

        Action<IServiceCollection> eventSerializerRegistrator = services => services.AddSingleton<IEventSerializer, NewtonsoftEventSerializer>();
        Action<IServiceCollection> typeRegistryRegistrator = services => services.AddSingleton<ITypeRegistry, TypeRegistry>();

        bool compression = false;
        bool schemaProvisioningEnabled = false;
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

        public IStreamStoreConfigurator WithSingleTenant(Action<ISingleTenantDatabaseRegistrator> registrator)
        {
            var singleTenantRegistrator = new SingleTenantDatabaseRegistrator();
            registrator(singleTenantRegistrator);
            this.dbRegistrator = singleTenantRegistrator;
            return this;
        }

        public IStreamStoreConfigurator WithMultitenancy(Action<IMultitenantDatabaseRegistrator> registrator)
        {
            var multiTenantregistrator = new MultitenantDatabaseRegistrator();
            registrator(multiTenantregistrator);
            this.dbRegistrator = multiTenantregistrator;
            return this;
        }

        public IServiceCollection Configure(IServiceCollection services)
        {
            if (dbRegistrator == null)
                throw new InvalidOperationException("Database backend is not registered");

            var configuration = CreateConfiguration();

            dbRegistrator.Apply(services);

            eventSerializerRegistrator(services);
            typeRegistryRegistrator(services);

            RegisterStoreDependencies(services, configuration);

            if (configuration.SchemaProvisioningEnabled)
                services.AddHostedService<SchemaProvisioningService>();

            return services;
        }

        private StreamStoreConfiguration CreateConfiguration()
        {
            return new StreamStoreConfiguration
            {
                ReadingMode = mode,
                ReadingPageSize = pageSize,
                CompressionEnabled = compression,
                SchemaProvisioningEnabled = schemaProvisioningEnabled
            };
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
