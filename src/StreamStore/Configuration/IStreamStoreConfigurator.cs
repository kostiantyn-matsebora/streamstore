using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;


namespace StreamStore
{
    public interface IStreamStoreConfigurator
    {
        IStreamStoreConfigurator WithReadingMode(StreamReadingMode mode);


        IStreamStoreConfigurator WithReadingPageSize(int pageSize);

        IStreamStoreConfigurator ConfigureSerialization(Action<ISerializationConfigurator> configure);

        IStreamStoreConfigurator WithSingleStorage(Action<ISingleTenantConfigurator> configure);

        IStreamStoreConfigurator WithMultitenancy(Action<IMultitenancyConfigurator> configure);

        IStreamStoreConfigurator EnableSchemaProvisioning();

        IServiceCollection Configure(IServiceCollection services);
    }
}
