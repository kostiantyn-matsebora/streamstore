
namespace StreamStore.Multitenancy
{
    public interface ITenantStreamDatabaseProvider
    {
        IStreamDatabase GetDatabase(Id tenantId);
    }
}
