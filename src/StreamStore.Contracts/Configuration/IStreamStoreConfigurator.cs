using System;
using Microsoft.Extensions.DependencyInjection;


namespace StreamStore
{
    public interface IStreamStoreConfigurator
    {
        public IStreamStoreConfigurator WithReadingMode(StreamReadingMode mode);


        public IStreamStoreConfigurator WithReadingPageSize(int pageSize);

        IStreamStoreConfigurator ConfigureSerialization(Action<ISerializationConfigurator> configure);

        public IStreamStoreConfigurator WithSingleDatabse(Action<ISingleTenantDatabaseConfigurator> configure);

        public IStreamStoreConfigurator WithMultitenancy(Action<IMultitenantDatabaseConfigurator> configure);

        public IServiceCollection Configure(IServiceCollection services);
    }
}
