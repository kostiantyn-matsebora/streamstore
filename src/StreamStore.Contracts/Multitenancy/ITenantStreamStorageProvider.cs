
namespace StreamStore.Multitenancy
{
    public interface ITenantStreamStorageProvider
    {
        IStreamStorage GetStorage(Id tenantId);
    }
}
