
using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Provisioning;
using StreamStore.Serialization;


namespace StreamStore
{

    public class StreamStoreConfigurator : IStreamStoreConfigurator
    {
        StreamReadingMode mode = StreamReadingMode.Default;
        IStreamDatabaseConfigurator? databaseConfigurator;
        ISerializationConfigurator serializationConfigurator = new SerializationConfigurator();

        Action<IServiceCollection> typeRegistryRegistrator = services => services.AddSingleton<ITypeRegistry, TypeRegistry>();

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

        public IStreamStoreConfigurator ConfigureSerialization(Action<ISerializationConfigurator> configure)
        {
            configure(serializationConfigurator);
            return this;
        }

        public IStreamStoreConfigurator EnableSchemaProvisioning()
        {
            schemaProvisioningEnabled = true;
            return this;
        }

        public IStreamStoreConfigurator WithSingleTenant(Action<ISingleTenantDatabaseConfigurator> configure)
        {
            var configurator = new SingleTenantDatabaseConfigurator();
            configure(configurator);
            databaseConfigurator = configurator;
            return this;
        }

        public IStreamStoreConfigurator WithMultitenancy(Action<IMultitenantDatabaseConfigurator> configure)
        {
            var configurator = new MultitenantDatabaseConfigurator();
            configure(configurator);
            databaseConfigurator = configurator;
            return this;
        }

        public IServiceCollection Configure(IServiceCollection services)
        {
            if (databaseConfigurator == null)
                throw new InvalidOperationException("Database backend is not registered");

            var configuration = CreateConfiguration();

            CopyServices(databaseConfigurator.Configure(), services);
            CopyServices(serializationConfigurator.Configure(), services);

            typeRegistryRegistrator(services);

            RegisterStoreDependencies(services, configuration);

            if (configuration.SchemaProvisioningEnabled) services.AddHostedService<SchemaProvisioningService>();

            return services;
        }

        void CopyServices(IServiceCollection source, IServiceCollection destination)
        {
            foreach (var service in source)
            {
                destination.Add(service);
            }
        }

        private StreamStoreConfiguration CreateConfiguration()
        {
            return new StreamStoreConfiguration
            {
                ReadingMode = mode,
                ReadingPageSize = pageSize,
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
