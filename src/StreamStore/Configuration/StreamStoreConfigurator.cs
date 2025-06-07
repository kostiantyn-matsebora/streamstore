using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Extensions;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;
using StreamStore.Serialization;
using StreamStore.Store;


namespace StreamStore.Configuration
{
    internal class StreamStoreConfigurator : IStreamStoreConfigurator
    {
        readonly ISerializationConfigurator serializationConfigurator = new SerializationConfigurator();

        IServiceCollection serializationServices;
        IServiceCollection? storageServices;
        IServiceCollection storeServices =  new ServiceCollection();

        StreamStoreConfiguration config = new StreamStoreConfiguration();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public StreamStoreConfigurator()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            RegisterDefaultSerialization();
            RegisterStore();
        }

        

        public IStreamStoreConfigurator WithReadingMode(StreamReadingMode mode)
        {
            config.ReadingMode = mode;
            return this;
        }

        public IStreamStoreConfigurator WithReadingPageSize(int pageSize)
        {
            config.ReadingPageSize = pageSize;
            return this;
        }

        public IStreamStoreConfigurator ConfigureSerialization(Action<ISerializationConfigurator> configure)
        {
            configure(serializationConfigurator);
            serializationServices = serializationConfigurator.Configure();
            return this;
        }

        public IStreamStoreConfigurator EnableAutomaticProvisioning()
        {
            config.ProvisioningEnabled = true;
            return this;
        }

        public IStreamStoreConfigurator ConfigurePersistence(Action<IServiceCollection> configure)
        {
            storageServices = new ServiceCollection();
            configure(storageServices);
            return this;
        }

        public IServiceCollection Configure(IServiceCollection services)
        {
            if (storageServices == null)
                throw new InvalidOperationException("Persistence is not configured. Use ConfigurePersistence to register storage services.");

   
            if (config.ProvisioningEnabled)
                RegisterSchemaProvisioning(services);

            // Copy serialization and storage services
            services
                .CopyFrom(storeServices)
                .CopyFrom(serializationServices)
                .CopyFrom(storageServices);

            return services;
        }

        IServiceCollection RegisterSchemaProvisioning(IServiceCollection services)
        {
            return config.MultitenancyEnabled switch
            {
                true => services.AddHostedService<TenantSchemaProvisioningService>(),
                false => services.AddHostedService<SchemaProvisioningService>()
            };
        }

        public IStreamStoreConfigurator EnableMultitenancy<TProvider>() where TProvider : class, ITenantProvider
        {
            config.MultitenancyEnabled = true;
            storeServices.AddSingleton<ITenantProvider, TProvider>();
            storeServices.AddSingleton<ITenantStreamStoreFactory, TenantStreamStoreFactory>();
            return this;
        }

        public IStreamStoreConfigurator EnableMultitenancy(params Id[] tenants)
        {
            if (tenants == null || tenants.Length == 0)
                throw new ArgumentException("Tenants cannot be null or empty.", nameof(tenants));
            
            config.MultitenancyEnabled = true;

            storeServices.AddSingleton<ITenantProvider>(new DefaultTenantProvider(tenants));
            storeServices.AddSingleton<ITenantStreamStoreFactory, TenantStreamStoreFactory>();
            return this;
        }

        void RegisterDefaultSerialization()
        {
            serializationServices = serializationConfigurator.Configure();
        }


        void RegisterStore()
        {
            storeServices.RegisterDomainValidation();

            storeServices
                    .AddSingleton(config)
                    .AddSingleton<StreamEventEnumeratorFactory>()
                    .AddSingleton<IEventConverter, EventConverter>()
                    .AddSingleton<IStreamUnitOfWorkFactory, StreamUnitOfWorkFactory>()
                    .AddSingleton<IStreamStore, StreamStore>();
        }

    }
}
