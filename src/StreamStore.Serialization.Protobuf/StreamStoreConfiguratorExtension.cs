namespace StreamStore.Serialization.Protobuf
{
    public static class StreamStoreConfiguratorExtension
    {
        public static IStreamStoreConfigurator WithProtobufSerializer(this IStreamStoreConfigurator configurator, bool compression = true)
        {
            return configurator.WithEventSerializer<ProtobufEventSerializer>();
        }
    }
}
