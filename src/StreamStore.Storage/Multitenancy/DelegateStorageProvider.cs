using System;
using StreamStore.Extensions;
using StreamStore.Multitenancy;

namespace StreamStore.Storage.Multitenancy
{
    internal class DelegateStorageProvider : ITenantStreamStorageProvider
    {
        readonly Func<Id, IStreamStorage> provider;
        public DelegateStorageProvider(Func<Id, IStreamStorage> provider)
        {
            this.provider = provider.ThrowIfNull(nameof(provider));
        }

        public IStreamStorage GetStorage(Id tenantId)
        {
            return provider(tenantId);
        }
    }
}
