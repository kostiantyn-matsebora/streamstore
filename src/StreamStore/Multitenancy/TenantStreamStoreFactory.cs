using StreamStore.Extensions;
using StreamStore.Store;
using StreamStore.Validation;

namespace StreamStore.Multitenancy
{
    class TenantStreamStoreFactory : ITenantStreamStoreFactory
    {
        readonly StreamStoreConfiguration configuration;
        readonly ITenantStreamStorageProvider storageProvider;
        readonly IEventConverter converter;
        private readonly IStreamMutationRequestValidator validator;

        public TenantStreamStoreFactory(StreamStoreConfiguration configuration, ITenantStreamStorageProvider storageProvider, IEventConverter converter, IStreamMutationRequestValidator validator)
        {
           this.configuration = configuration.ThrowIfNull(nameof(configuration));
           this.storageProvider = storageProvider.ThrowIfNull(nameof(storageProvider));
           this.converter = converter.ThrowIfNull(nameof(converter));
           this.validator = validator.ThrowIfNull(nameof(validator));
        }

        public IStreamStore Create(Id tenantId)
        {
            var storage = storageProvider.GetStorage(tenantId);

            var enumeratorFactory = new StreamEventEnumeratorFactory(configuration, storage, converter);
            var uowFactory = new StreamUnitOfWorkFactory(storage, converter, validator);
            return new StreamStore(enumeratorFactory, configuration, storage, uowFactory);
        }
    }
}
