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

        INewStreamStoreConfigurator EnableMultitenancy<TProvider>() where TProvider : class, ITenantProvider;

        INewStreamStoreConfigurator EnableMultitenancy(params Id[] tenants);

        INewStreamStoreConfigurator EnableAutomaticProvisioning();

        INewStreamStoreConfigurator ConfigureStreamPersistence(Action<IServiceCollection> configure);
    }
}
