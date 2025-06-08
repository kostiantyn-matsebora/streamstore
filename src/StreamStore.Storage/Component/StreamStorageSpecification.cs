using StreamStore.Configuration;
using StreamStore.Provisioning;

namespace StreamStore.Storage.Component
{
    public sealed class StreamStorageSpecification: ComponentSpecificationBase
    {
        public StreamStorageSpecification(bool enableProvisioning)
        {
            AddRequiredDependency<IStreamStorage>();
            if (enableProvisioning)
                AddRequiredDependency<ISchemaProvisioner>();
        }
    }
}
