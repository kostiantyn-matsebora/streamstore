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
        IServiceCollection storeServices =  new ServiceCollection();

        StreamStoreConfiguration config = new StreamStoreConfiguration();
        StreamStorageMode mode = StreamStorageMode.Single;

        bool provisioningEnabled;

        public NewStreamStoreConfigurator()
        {
            serializationServices = serializationConfigurator.Configure();
            ConfigureStore();
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

        public INewStreamStoreConfigurator EnableAutomaticProvisioning()
        {
            provisioningEnabled = true;
            return this;
        }

        public INewStreamStoreConfigurator ConfigurePersistence(Action<IServiceCollection> configure)
        {
            storageServices = new ServiceCollection().AddSingleton(mode);
            configure(storageServices);
            return this;
        }

        public IServiceCollection Configure(IServiceCollection services)
        {
            if (storageServices == null)
                throw new InvalidOperationException("Persistence is not configured. Use ConfigurePersistence to register storage services.");

            services.AddSingleton(config);
   
            if (provisioningEnabled)
                RegisterSchemaProvisioning(services);

            // Copy serialization and storage services
            services
                .CopyFrom(storeServices)
                .CopyFrom(serializationServices)
                .CopyFrom(storageServices);

            return services;
        }

        void ConfigureStore()
        {
            storeServices.RegisterDomainValidation();

            storeServices
                    .AddSingleton<StreamEventEnumeratorFactory>()
                    .AddSingleton<IEventConverter, EventConverter>()
                    .AddSingleton<IStreamUnitOfWorkFactory, StreamUnitOfWorkFactory>()
                    .AddSingleton<IStreamStore, StreamStore>();
        }

        void RegisterSchemaProvisioning(IServiceCollection services)
        {
            if (mode == StreamStorageMode.Multitenant)
            {
                services.AddHostedService<TenantSchemaProvisioningService>();
            }
            else
            {
                services.AddHostedService<SchemaProvisioningService>();
            }
        }

        public INewStreamStoreConfigurator EnableMultitenancy<TProvider>() where TProvider : class, ITenantProvider
        {
            mode = StreamStorageMode.Multitenant;
            storeServices.AddSingleton<ITenantProvider, TProvider>();
            storeServices.AddSingleton<ITenantStreamStoreFactory, TenantStreamStoreFactory>();
            return this;
        }

        public INewStreamStoreConfigurator EnableMultitenancy(params Id[] tenants)
        {
            if (tenants == null || tenants.Length == 0)
                throw new ArgumentException("Tenants cannot be null or empty.", nameof(tenants));

            mode = StreamStorageMode.Multitenant;
            storeServices.AddSingleton<ITenantProvider>(new DefaultTenantProvider(tenants));
            storeServices.AddSingleton<ITenantStreamStoreFactory, TenantStreamStoreFactory>();
            return this;
        }
    }
}
