using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;


namespace StreamStore
{
    public interface IStreamStoreConfigurator
    {
        public IStreamStoreConfigurator WithReadingMode(StreamReadingMode mode);


        public IStreamStoreConfigurator WithReadingPageSize(int pageSize);

        IStreamStoreConfigurator ConfigureSerialization(Action<ISerializationConfigurator> configure);

        public IStreamStoreConfigurator WithSingleTenant(Action<ISingleTenantDatabaseConfigurator> configure);

        public IStreamStoreConfigurator WithMultitenancy(Action<IMultitenantDatabaseConfigurator> configure);
    }
}
