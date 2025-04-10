namespace StreamStore.Multitenancy
{
    public interface ITenantStreamStoreFactory
    {
        IStreamStore Create(Id tenantId);
    }
}
