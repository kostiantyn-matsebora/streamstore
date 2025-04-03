
namespace StreamStore.Multitenancy
{
    public interface ITenantStreamDatabaseProvider
    {
        IStreamStorage GetDatabase(Id tenantId);
    }
}
