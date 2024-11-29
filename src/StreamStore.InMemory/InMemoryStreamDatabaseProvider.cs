using System.Collections.Concurrent;
using StreamStore.Multitenancy;

namespace StreamStore.InMemory
{
    public sealed class InMemoryStreamDatabaseProvider : ITenantStreamDatabaseProvider
    {
        internal ConcurrentDictionary<string, InMemoryStreamDatabase> registry = new ConcurrentDictionary<string, InMemoryStreamDatabase>();

        public IStreamDatabase GetDatabase(Id tenantId)
        {
            return registry.GetOrAdd(tenantId, _ => new InMemoryStreamDatabase());
        }
    }
}
