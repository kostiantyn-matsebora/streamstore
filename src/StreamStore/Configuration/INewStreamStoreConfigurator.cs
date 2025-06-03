using System;
using Microsoft.Extensions.DependencyInjection;
using StreamStore.Serialization;


namespace StreamStore
{
    public interface INewStreamStoreConfigurator
    {
        INewStreamStoreConfigurator WithReadingMode(StreamReadingMode mode);


        INewStreamStoreConfigurator WithReadingPageSize(int pageSize);

        INewStreamStoreConfigurator ConfigureSerialization(Action<ISerializationConfigurator> configure);

        INewStreamStoreConfigurator EnableMultitenancy();

        INewStreamStoreConfigurator EnableSchemaProvisioning();

        INewStreamStoreConfigurator ConfigureStorage(Action<IServiceCollection> configure);
    }
}
