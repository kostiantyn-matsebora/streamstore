namespace StreamStore.Storage.EventFlow
{
    public interface ITenantIdResolver
    {
        public Id Resolve();
    }
}
