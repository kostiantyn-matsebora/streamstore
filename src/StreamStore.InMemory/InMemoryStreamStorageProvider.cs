using System.Collections.Concurrent;
using StreamStore.Multitenancy;

namespace StreamStore.InMemory
{
    public sealed class InMemoryStreamStorageProvider : ITenantStreamStorageProvider
    {
        internal ConcurrentDictionary<string, InMemoryStreamStorage> registry = new ConcurrentDictionary<string, InMemoryStreamStorage>();

        public IStreamStorage GetStorage(Id tenantId)
        {
            return registry.GetOrAdd(tenantId, _ => new InMemoryStreamStorage());
        }
    }
}
