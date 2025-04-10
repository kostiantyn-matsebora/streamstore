using StreamStore.Configuration.Storage;

namespace StreamStore.Configuration
{
    public static class ConfiguratorFactory
    {
        public static IStreamStoreConfigurator StoreConfigurator => new StreamStoreConfigurator();
        public static ISingleTenantConfigurator SingleTenantConfigurator => new SingleTenantConfigurator();
        public static IMultitenancyConfigurator MultitenancyConfigurator => new MultitenancyConfigurator();
    }
}
