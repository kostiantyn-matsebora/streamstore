using StreamStore.Serialization;

namespace StreamStore {
 
 public static class StreamStoreConfiguratorExtension {
    public static StreamStoreConfigurator WithTextJsonSerializer(this StreamStoreConfigurator configurator) {
        configurator.WithEventSerializer<SystemTextJsonEventSerializer>();
        return configurator;
    }
 }
}
