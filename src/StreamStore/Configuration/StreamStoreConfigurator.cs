
using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Configuration;
using StreamStore.Configuration.Database;
using StreamStore.Provisioning;
using StreamStore.Serialization;


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

            RegisterStoreDependencies(services, configuration);
            RegisterSchemaProvisioning(services);

            return services;
        }

        void CopyServices(IServiceCollection source, IServiceCollection destination)
        {
            foreach (var service in source)
            {
                destination.Add(service);
            }
        }

        void RegisterSchemaProvisioning(IServiceCollection services)
        {
            var type = databaseConfigurator!.GetType();
            switch (type)
            {
                case Type _ when type == typeof(SingleTenantDatabaseConfigurator):
                    services.AddHostedService<SchemaProvisioningService>();
                    break;
                case Type _ when type == typeof(MultitenantDatabaseConfigurator):
                    services.AddHostedService<TenantSchemaProvisioningService>();
                    break;
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

        static void RegisterStoreDependencies(IServiceCollection services, StreamStoreConfiguration configuration)
        {
            services
                .AddSingleton(configuration)
                .AddSingleton<StreamEventEnumeratorFactory>()
                .AddSingleton<EventConverter>()
                .AddSingleton<IStreamStore, StreamStore>();
        }
    }
}
