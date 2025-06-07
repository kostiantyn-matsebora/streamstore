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

        IStreamStoreConfigurator EnableMultitenancy<TProvider>() where TProvider : class, ITenantProvider;

        IStreamStoreConfigurator EnableMultitenancy(params Id[] tenants);

        IStreamStoreConfigurator EnableAutomaticProvisioning();

        IStreamStoreConfigurator ConfigurePersistence(Action<IServiceCollection> configure);
    }
}
