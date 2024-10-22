using StreamStore.Serialization;


namespace StreamStore
{

    public static class StreamStoreConfiguratorExtension {
    public static IStreamStoreConfigurator WithTextJsonSerializer(this IStreamStoreConfigurator configurator) {
        configurator.WithEventSerializer<SystemTextJsonEventSerializer>();
        return configurator;
    }
 }
}
