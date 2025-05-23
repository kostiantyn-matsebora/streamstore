using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Configuration.Storage;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using StreamStore.Storage.Validation;
using StreamStore.Store;
using StreamStore.Validation;



namespace StreamStore
{

    class StreamStoreConfigurator : IStreamStoreConfigurator
    {
        StreamReadingMode mode = StreamReadingMode.Default;
        IStreamStorageConfigurator? storageConfigurator;
        readonly ISerializationConfigurator serializationConfigurator = new SerializationConfigurator();

        bool schemaProvisioningEnabled = false;
        int pageSize = 1_000;

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

        public IStreamStoreConfigurator WithSingleStorage(Action<ISingleTenantConfigurator> configure)
        {
            var configurator = new SingleTenantConfigurator();
            configure(configurator);
            storageConfigurator = configurator;
            return this;
        }

        public IStreamStoreConfigurator WithMultitenancy(Action<IMultitenancyConfigurator> configure)
        {
            var configurator = new MultitenancyConfigurator();
            configure(configurator);
            storageConfigurator = configurator;
            return this;
        }

        public IServiceCollection Configure(IServiceCollection services)
        {
            if (storageConfigurator == null)
                throw new InvalidOperationException("Storage backend is not registered");

            var configuration = CreateConfiguration();

            CopyServices(storageConfigurator.Configure(), services);
            CopyServices(serializationConfigurator.Configure(), services);

            RegisterSharedDependencies(services, configuration);
            RegisterModeDependencies(services, (dynamic)storageConfigurator);

            return services;
        }

        static void CopyServices(IServiceCollection source, IServiceCollection destination)
        {
            foreach (var service in source)
            {
                destination.Add(service);
            }
        }



        StreamStoreConfiguration CreateConfiguration()
        {
            return new StreamStoreConfiguration
            {
                ReadingMode = mode,
                ReadingPageSize = pageSize,
            };
        }

        static void RegisterSharedDependencies(IServiceCollection services, StreamStoreConfiguration configuration)
        {
            services
                .AddSingleton(configuration)
                .AddSingleton<StreamEventEnumeratorFactory>()
                .AddSingleton<IEventConverter, EventConverter>()
                .AddSingleton<IStreamUnitOfWorkFactory, StreamUnitOfWorkFactory>()
                .AddSingleton<IStreamStore, StreamStore>();
        }

#pragma warning disable S1172 // Unused method parameters should be removed
        void RegisterModeDependencies(IServiceCollection services, SingleTenantConfigurator configurator)
        {
            if (schemaProvisioningEnabled) services.AddHostedService<SchemaProvisioningService>();
        }

        void RegisterModeDependencies(IServiceCollection services, MultitenancyConfigurator configurator)

        {
            services.AddSingleton<ITenantStreamStoreFactory, TenantStreamStoreFactory>();
            if (schemaProvisioningEnabled) services.AddHostedService<TenantSchemaProvisioningService>();
        }

#pragma warning restore S1172 // Unused method parameters should be removed
    }
}
