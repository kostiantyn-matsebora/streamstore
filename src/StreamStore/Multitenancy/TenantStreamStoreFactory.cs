namespace StreamStore.Multitenancy
{
    internal class TenantStreamStoreFactory : ITenantStreamStoreFactory
    {
        readonly StreamStoreConfiguration configuration;
        readonly ITenantStreamStorageProvider storageProvider;
        readonly IEventSerializer serializer;

        public TenantStreamStoreFactory(StreamStoreConfiguration configuration, ITenantStreamStorageProvider storageProvider, IEventSerializer serializer)
        {
           this.configuration = configuration.ThrowIfNull(nameof(configuration));
           this.storageProvider = storageProvider.ThrowIfNull(nameof(storageProvider));
           this.serializer = serializer.ThrowIfNull(nameof(serializer));
        }
        public IStreamStore Create(Id tenantId)
        {
            return new StreamStore(configuration, storageProvider.GetStorage(tenantId), serializer);
        }
    }
}
