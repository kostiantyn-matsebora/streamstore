namespace StreamStore.Multitenancy
{
    internal class TenantStreamStoreFactory : ITenantStreamStoreFactory
    {
        readonly StreamStoreConfiguration configuration;
        readonly ITenantStreamDatabaseProvider databaseProvider;
        readonly IEventSerializer serializer;

        public TenantStreamStoreFactory(StreamStoreConfiguration configuration, ITenantStreamDatabaseProvider databaseProvider, IEventSerializer serializer)
        {
           this.configuration = configuration.ThrowIfNull(nameof(configuration));
           this.databaseProvider = databaseProvider.ThrowIfNull(nameof(databaseProvider));
           this.serializer = serializer.ThrowIfNull(nameof(serializer));
        }
        public IStreamStore Create(Id tenantId)
        {
            return new StreamStore(configuration, databaseProvider.GetDatabase(tenantId), serializer);
        }
    }
}
