using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using StreamStore.Serialization;
using StreamStore.Storage;
using StreamStore.Store;


namespace StreamStore.Configuration
{
    internal class NewStreamStoreConfigurator : INewStreamStoreConfigurator
    {
        readonly ISerializationConfigurator serializationConfigurator = new SerializationConfigurator();

        IServiceCollection serializationServices;
        IServiceCollection? storageServices;

        StreamStoreConfiguration config = new StreamStoreConfiguration();
        StreamStorageMode storageMode = StreamStorageMode.Single;

        private bool provisioningEnabled;

        public NewStreamStoreConfigurator()
        {
            serializationServices = serializationConfigurator.Configure();
        }

        public INewStreamStoreConfigurator WithReadingMode(StreamReadingMode mode)
        {
            config.ReadingMode = mode;
            return this;
        }

        public INewStreamStoreConfigurator WithReadingPageSize(int pageSize)
        {
            config.ReadingPageSize = pageSize;
            return this;
        }

        public INewStreamStoreConfigurator ConfigureSerialization(Action<ISerializationConfigurator> configure)
        {
            configure(serializationConfigurator);
            serializationServices = serializationConfigurator.Configure();
            return this;
        }

        public INewStreamStoreConfigurator EnableMultitenancy()
        {
            storageMode = StreamStorageMode.Multitenant;
            return this;
        }

        public INewStreamStoreConfigurator EnableSchemaProvisioning()
        {
            provisioningEnabled = true;
            return this;
        }

        public INewStreamStoreConfigurator ConfigureStorage(Action<IServiceCollection> configure)
        {
            storageServices =
                new ServiceCollection()
                .AddSingleton(storageMode);

            configure(storageServices);
            return this;
        }

        public IServiceCollection Configure(IServiceCollection services)
        {
            if (storageServices == null)
                throw new InvalidOperationException("Persistence is not configured. Use ConfigureStorage to register storage services.");

            // Register stream store configuration
            RegisterStoreConfiguration(services);

            // Copy serialization and storage services
            services
                .CopyFrom(serializationServices)
                .CopyFrom(storageServices);

            return services;
        }

        void RegisterStoreConfiguration(IServiceCollection services)
        {
            services.RegisterDomainValidation();

            services.AddSingleton(config)
                    .AddSingleton<StreamEventEnumeratorFactory>()
                    .AddSingleton<IEventConverter, EventConverter>()
                    .AddSingleton<IStreamUnitOfWorkFactory, StreamUnitOfWorkFactory>()
                    .AddSingleton<IStreamStore, StreamStore>();


            RegisterMultitenancy(services);

            if (provisioningEnabled)
                RegisterSchemaProvisioning(services);
        }

        void RegisterMultitenancy(IServiceCollection services)
        {
            if (storageMode == StreamStorageMode.Multitenant)
            {
                services.AddSingleton<ITenantStreamStoreFactory, TenantStreamStoreFactory>();
            }
        }

        void RegisterSchemaProvisioning(IServiceCollection services)
        {
            if (storageMode == StreamStorageMode.Multitenant)
            {
                services.AddHostedService<TenantSchemaProvisioningService>();
            }
            else
            {
                services.AddHostedService<SchemaProvisioningService>();
            }
        }
    }
}
