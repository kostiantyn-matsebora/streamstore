using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Configuration.Database;
using StreamStore.Multitenancy;
using StreamStore.Provisioning;



namespace StreamStore
{

    public class StreamStoreConfigurator : IStreamStoreConfigurator
    {
        StreamReadingMode mode = StreamReadingMode.Default;
        IStreamDatabaseConfigurator? databaseConfigurator;
        readonly ISerializationConfigurator serializationConfigurator = new SerializationConfigurator();

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

        public IStreamStoreConfigurator WithSingleDatabse(Action<ISingleTenantConfigurator> configure)
        {
            var configurator = new SingleTenantConfigurator();
            configure(configurator);
            databaseConfigurator = configurator;
            return this;
        }

        public IStreamStoreConfigurator WithMultitenancy(Action<IMultitenancyConfigurator> configure)
        {
            var configurator = new MultitenancyConfigurator();
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

            RegisterSharedDependencies(services, configuration);
            RegisterModeDependencies(services, (dynamic)databaseConfigurator);

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
                .AddSingleton<EventConverter>()
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
            if (schemaProvisioningEnabled)  services.AddHostedService<TenantSchemaProvisioningService>();
        }
        #pragma warning restore S1172 // Unused method parameters should be removed
    }
}